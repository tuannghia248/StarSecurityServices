using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarSecurityService.Models
{
    [System.Runtime.Serialization.DataContract(IsReference = true)]
    [System.ComponentModel.DataAnnotations.ScaffoldTable(true)]
    public class VacancyMetaData
    {
        [Display(Name = "ID")]
        public int id;

        [Required]
        [Display(Name = "Job Title")]
        public string job { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string description { get; set; }

        [Required]
        [Display(Name = "Quantity Needed")]
        public int quantity { get; set; }

        [Required]
        [Display(Name = "Requirement")]
        public string requirement { get; set; }

        [Required]
        [Display(Name = "Salary (VND)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public float salary { get; set; }

        [Required]
        [Display(Name = "Deadline")]
        public string deadline { get; set; }
    }
}