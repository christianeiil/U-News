using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace U_News.Models
{
    public class Events
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Event Name")]
        [MaxLength(50)]
        [Required]
        public string EventName { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000)]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Location")]
        [MaxLength(50)]
        [Required]
        public string Location { get; set; }

       

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateAdded { get; set; }

        



    }
}