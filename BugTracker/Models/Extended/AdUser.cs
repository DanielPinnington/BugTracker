using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BugTracker.Models
{
    [MetadataType(typeof(UserMetadataAdmin))]
    public partial class Admin
    {
        public string ConfirmPassword { get; set; }
    }

   
    public class UserMetadataAdmin
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required, MaxLength(16)] //Max length of username is 16
        public String LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email ID")]
        [Required, MaxLength(30)] //Max length of username is 30
        public String EmailID { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20)] //Can add MinimumLength StringLength(20), MinimumLength(4)
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not the same")]
        public String ConfirmPassword { get; set; }
    }
}