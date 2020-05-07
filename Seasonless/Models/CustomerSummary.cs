using System.ComponentModel.DataAnnotations.Schema;

namespace Seasonless.Models
{
    public class CustomerSummary
    {
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Season")]
        public int SeasonID { get; set; }

        public int Credit { get; set; }
        public int TotalRepaid { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int RemainingCredit => Credit - TotalRepaid;

        public virtual Season Season { get; set; }
        public virtual Customer Customer { get; set; }
    }
}