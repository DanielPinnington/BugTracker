using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class UserLogin
    {
        [Display(Name = "Username ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username Required")]

        public string usernameID { get; set; }
    }
}