using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seasonless.Models
{
    public class Season
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SeasonID { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
    }
}