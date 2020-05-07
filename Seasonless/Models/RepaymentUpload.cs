using System;
using System.ComponentModel.DataAnnotations;

namespace Seasonless.Models
{
    public class RepaymentUpload
    {
        [Key]
        public int RepaymentID { get; set; }

        public int CustomerID { get; set; }

        public int SeasonID { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public bool IsValid { get; set; } = true;

        public Reason? Reason { get; set; } = null;
    }
}