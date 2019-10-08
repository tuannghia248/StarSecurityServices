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

        [Display(Name = "Job Title")]
        public string job { get; set; }

        [Display(Name = "Position")]
        public string position { get; set; }

        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        [Display(Name = "Deadline")]
        public string deadline { get; set; }
    }
}