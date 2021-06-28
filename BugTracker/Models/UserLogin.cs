using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class UserLogin
    {  
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username Required")]
        public string emailID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}