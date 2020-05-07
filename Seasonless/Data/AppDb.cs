using Microsoft.EntityFrameworkCore;
using Seasonless.Models;

namespace Seasonless.Data
{
    public class AppDb : DbContext
    {
        public AppDb()
        {
        }

        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSummary>()
                .HasKey(c => new { c.CustomerID, c.SeasonID });
        }

        public DbSet<Season> Seasons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerSummary> Summaries { get; set; }
        public DbSet<RepaymentUpload> RepaymentUploads { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
    }
}