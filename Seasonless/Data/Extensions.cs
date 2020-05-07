﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Seasonless.Models;

namespace Seasonless.Data
{
    public static class Extensions
    {
        public static bool Exists<TEntity>(this AppDb context, Func<TEntity, bool> condition) where TEntity : class
        {
            return context.Set<TEntity>().Any(condition);
        }

        public static void Seed(this IApplicationBuilder app, string path)
        {
            var data = JsonConvert.DeserializeObject<DataViewModel>(System.IO.File.ReadAllText(path));
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDb>();
                if (!context.Customers.Any())
                {
                    context.AddRange(data.Customers);
                    context.SaveChanges();
                }

                if (!context.Seasons.Any())
                {
                    context.AddRange(data.Seasons);
                    context.SaveChanges();
                }

                if (!context.Summaries.Any())
                {
                    context.AddRange(data.CustomerSummaries);
                    context.SaveChanges();
                }

                if (!context.Repayments.Any())
                {
                    for (var index = 0; index < data.RepaymentUploads.Count; index++)
                    {
                        context.ProcessPaymentAt(data.RepaymentUploads, index);
                    }
                }
            }
        }

        public static void ProcessPaymentAt(this AppDb context, IList<RepaymentUpload> uploads, int index)
        {
            var payment = uploads[index];
            if (!context.Exists<Customer>(c => c.CustomerID == payment.CustomerID))
            {
                payment.IsValid = false;
                payment.Reason = Reason.NoCustomer;
                context.Add(payment);
                context.SaveChanges();
                return;
            }

            if (payment.SeasonID != 0 && context.Exists<Season>(s => s.SeasonID == payment.SeasonID))
            {
                var summary = context.Summaries.FirstOrDefault(e =>
                    e.CustomerID == payment.CustomerID && e.SeasonID == payment.SeasonID);

                //Check if an existing customer has a dept in existing season.
                if (summary != null && summary.RemainingCredit > 0)
                {
                    //Add full amount to the customer summary with specified customer Id
                    //and season Id.
                    context.UpdateSummary(summary, payment.Amount);

                    //Create a single repayment record
                    context.AddRepayment(payment, payment.SeasonID);
                }
                else
                {
                    //Process as seasonless
                    payment.SeasonID = 0;
                    context.ProcessSeasonlessPayment(payment);
                }
            }
            else
            {
                context.ProcessSeasonlessPayment(payment);
            }
        }

        public static void ProcessSeasonlessPayment(this AppDb context, RepaymentUpload payment)
        {
            int? parentId = null;
            var amount = payment.Amount;

            //Get the customer summaries
            var summaries = context.Summaries
                .Where(e => e.CustomerID == payment.CustomerID)
                .Include(e => e.Season)
                .OrderBy(e => e.Season.StartDate).ToList();

            //Check if all debts are fulfilled add the overpayment to the last (max) season if any.
            //Or if there is only one season where there is outstanding debt.
            if (summaries.Count(e => e.Credit != e.TotalRepaid) == 1 ||
                summaries.All(e => e.Credit == e.TotalRepaid))
            {
                var summary = summaries[summaries.Count - 1];

                //Add full amount to the customer summary with specified customer Id
                //and season Id.
                context.UpdateSummary(summary, payment.Amount);

                //Create a single repayment record
                context.AddRepayment(payment, summary.SeasonID);
            }
            else if (summaries.Any(e => e.Credit != e.TotalRepaid))
            {
                summaries = summaries.Where(e => e.Credit != e.TotalRepaid).ToList();
                foreach (var summary in summaries)
                {
                    var remainingCredit = summary.RemainingCredit;
                    if (amount <= remainingCredit)
                    {
                        context.UpdateSummary(summary, amount);
                        context.AddRepayment(payment, summary.SeasonID, amount, parentId);
                        break;
                    }

                    if (!parentId.HasValue)
                    {
                        context.UpdateSummary(summary, remainingCredit);
                        parentId = context.AddRepayment(payment, summary.SeasonID, amount);

                        amount -= remainingCredit;

                        //Add adjustment record
                        context.AddRepayment(payment, summary.SeasonID, -amount, parentId);
                    }
                    else
                    {
                        context.UpdateSummary(summary, remainingCredit);

                        //Add adjustment record
                        context.AddRepayment(payment, summary.SeasonID, remainingCredit, parentId);

                        amount -= remainingCredit;
                    }
                }
            }
        }

        public static int AddRepayment(this DbContext context, RepaymentUpload payment, int seasonId, int? amount = null, int? parentId = null)
        {
            var repayment = new Repayment
            {
                CustomerID = payment.CustomerID,
                SeasonID = seasonId,
                Amount = amount ?? payment.Amount,
                Date = payment.Date,
                ParentID = parentId
            };

            context.Add(repayment);

            if (payment.RepaymentID == 0)
                context.Add(payment);

            context.SaveChanges();

            return repayment.RepaymentID;
        }

        public static void UpdateSummary(this DbContext context, CustomerSummary summary, int amount)
        {
            summary.TotalRepaid += amount;
            context.Update(summary);
            context.SaveChanges();
        }
    }
}