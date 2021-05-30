using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude ="IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                //Email already exists
                var doesExist = DoesEmailExist(user.EmailID);
                if (doesExist)
                {
                    ModelState.AddModelError("EmailExists", "Email already exists");
                    return View(user);
                }
            }
            else
            {
                message = "Invalid Request";
            }
            return View();
        }

        [NonAction]
        public bool DoesEmailExist(string emailID)
        {
            using(BugTrackerDBEntities dc = new BugTrackerDBEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }
    }
}