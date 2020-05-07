using System.Collections.Generic;

namespace Seasonless.Models
{
    public class DataViewModel
    {
        public IList<Season> Seasons { get; set; }
        public IList<Customer> Customers { get; set; }
        public IList<CustomerSummary> CustomerSummaries { get; set; }
        public IList<RepaymentUpload> RepaymentUploads { get; set; }
    }
}