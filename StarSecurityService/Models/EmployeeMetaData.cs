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
    public class EmployeeMetaData
    {
        [Display(Name = "ID")]
        public int id;
        [Required]
        [Display(Name = "Full Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string number { get; set; }
        [Required]
        [Display(Name = "Birthday")]
        public string birthday { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string position { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
        [Display(Name = "Qualification")]
        public string qualification { get; set; }
        [Display(Name = "Achievement")]
        public string achievement { get; set; }
        [Display(Name = "Department")]
        public int depantment_id { get; set; }
        [Display(Name = "Account")]
        public int account_id { get; set; }
        [Display(Name = "Contract")]
        public int contract_id { get; set; }
    }
}