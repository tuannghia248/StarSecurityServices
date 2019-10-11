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
    public class ServiceMetaData
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Required]
        [Display(Name = "Service Name")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "service name must be at least 6 characters.")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Service Description")]
        public string description { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
    }
}