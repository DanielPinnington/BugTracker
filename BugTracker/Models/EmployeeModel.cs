using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BugTracker.Controllers;

namespace BugTracker.Models
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        [Display(Name = "Username")]
        [Required, MaxLength(12)] //Max length of username is 12
        public String UsernameID { get; set; }

        [Display(Name = "First Name")]
        [Required, MaxLength(12)] //Max length of username is 12
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required, MaxLength(16)] //Max length of username is 16
        public String SecondName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required, MaxLength(30)] //Max length of username is 30
        public String EmailAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [Compare("EmailAddress", ErrorMessage = "The Email and Confirm Email Address must match")]
        public String ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20)] //Can add MinimumLength StringLength(20), MinimumLength(4)
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not the same")]
        public String ConfirmPassword { get; set; }

    }
}