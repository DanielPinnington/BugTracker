using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketSystemModel
    {

        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title of the Bug")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description for the Bug")]
        public string Description { get; set; }

        [Display(Name = "Current Date")]
        [Required]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Priority")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Low, Medium or High")]

        public Priority Priorities { get; set; }

    }
    public enum Priority
    {
        Low,
        Medium,
        High
    }
}