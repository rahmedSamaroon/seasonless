using System.ComponentModel.DataAnnotations;

namespace Seasonless.Models
{
    public enum Reason
    {
        [Display(Name = "There is no customer with the specified Id")]
        NoCustomer,

        [Display(Name = "There is no season with the specified Id")]
        NoSeason
    }
}