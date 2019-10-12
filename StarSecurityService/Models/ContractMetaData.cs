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
    public class ContractMetaData
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Code")]
        public string code { get; set; }

        [Required]
        [Display(Name = "Service")]
        public int service_id { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int client_id { get; set; }

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Display(Name = "Hired Employee Quantity")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Employee quantity must be greater than 0.")]
        public int quantity { get; set; }

        [Required(ErrorMessage = "The Duration field is required.")]
        [Display(Name = "Time Hiring (Day)")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Time hiring must be greater than 0.")]
        public int duration { get; set; }

        [Required]
        [Display(Name = "Total Price (VND)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public float price { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public string start_at { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string end_at { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Created Date")]
        public string created_at { get; set; }

        [Display(Name = "Updated Date")]
        public string updated_at { get; set; }
    }
}