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
    public class ClientMetaData
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Full name must be at least 6 characters.")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone number must be 10 or 11 numbers.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid phone number.")]
        public string phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email address.")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Service")]
        public int service_id { get; set; }

        [Display(Name = "Expected Employee Quantity")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Employee quantity must be greater than 0.")]
        public int quantity { get; set; }

        [Display(Name = "Intend Time Hiring")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Time hiring must be greater than 0.")]
        public int duration { get; set; }

        [Display(Name = "Expected Date")]
        [RegularExpression(@"(0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])[- \/.](19|20)\d\d", ErrorMessage = "Please enter valid date.")]
        public string start_at { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Created Date")]
        public string created_at { get; set; }

        [Display(Name = "Updated Date")]
        public string updated_at { get; set; }
    }
}