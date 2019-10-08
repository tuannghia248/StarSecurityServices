using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarSecurityService.Models
{
    public class AccountMetaData
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Username")]
        public string username { get; set; }

        [Display(Name = "Role")]
        public string role { get; set; }
    }
}