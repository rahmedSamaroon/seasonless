using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seasonless.Models
{
    public class Repayment
    {
        [Key]
        public int RepaymentID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Season")]
        public int SeasonID { get; set; }

        [ForeignKey("ParentRepayment")]
        public int? ParentID { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        [Display(Name = "Parent Repayment")]
        public virtual Repayment ParentRepayment { get; set; }

        public virtual Season Season { get; set; }
        public virtual Customer Customer { get; set; }
    }
}