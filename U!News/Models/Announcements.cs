using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace U_News.Models
{
    public class Announcements
    {
        [Key]

        public int ID { get; set; }

        [Display(Name = "User Type")]
        [Required]
        public int TypeID { get; set; }

        [Display(Name = "Title")]
        [Required]
        [MaxLength(200)]
        public string AN { get; set; }

        [Display(Name = "Details")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(1000)]
        public string Details { get; set; }

        public DateTime DateAdded { get; set; }
        
    }
}
