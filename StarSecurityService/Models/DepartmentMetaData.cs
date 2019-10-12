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
    public class DepartmentMetaData
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Department name must be at least 6 characters.")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Department Description")]
        public string description { get; set; }
    }
}