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
        [Display(Name = "Birthday")]
        [RegularExpression(@"(0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])[- \/.](19|20)\d\d", ErrorMessage = "Please enter valid birthday.")]
        public string birthday { get; set; }

        [Display(Name = "Position")]
        public string position { get; set; }

        [Display(Name = "Salary (VND)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public float salary { get; set; }

        [Display(Name = "Image")]
        public string image { get; set; }

        [Display(Name = "Qualification")]
        public string qualification { get; set; }

        [Display(Name = "Department")]
        public int depantment_id { get; set; }

        [Display(Name = "Account")]
        public int account_id { get; set; }

        [Display(Name = "Contract")]
        public int contract_id { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Join Date")]
        [RegularExpression(@"(0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])[- \/.](19|20)\d\d", ErrorMessage = "Please enter valid date.")]
        public string join_at { get; set; }

        [Display(Name = "Resign Date")]
        [RegularExpression(@"(0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])[- \/.](19|20)\d\d", ErrorMessage = "Please enter valid date.")]
        public string resign_at { get; set; }
    }
}