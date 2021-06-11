using BugTracker.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegistration([Bind(Exclude = "IsEmailVerified, ActivationCode")] Admin user)
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

                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;


                #region Save to Database
                using (BugTrackerDBEntities2 dc = new BugTrackerDBEntities2())
                {
                    dc.Admins.Add(user);
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();

                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link has been sent to your email ID:" + user.EmailID;
                    Status = true;

                }

                #endregion
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View();
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)//activation code
        {
            bool Status = false;

            using (BugTrackerDBEntities dc = new BugTrackerDBEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //This is to avoid confirm password (does not match) issue
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = true;
            return View();
        }

        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnURL = "")
        {
            string message = "";
            using (BugTrackerDBEntities dc = new BugTrackerDBEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == login.emailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //If remember me, remember this for one year! (525600)
                        var ticket = new FormsAuthenticationTicket(login.emailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout); //ONE YEAR (TIMEOUT)
                        cookie.HttpOnly = true;

                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnURL))
                        {
                            return Redirect(ReturnURL);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    message = "Invalid Credentials";
                }
            }
            ViewBag.Message = "";

            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();


            return RedirectToAction("Login", "User");
        }



        [NonAction]
        public bool DoesEmailExist(string emailID)
        {
            using (BugTrackerDBEntities dc = new BugTrackerDBEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var scheme = Request.Url.Scheme;
            var host = Request.Url.Host;
            var port = Request.Url.Port;

            var verifyURL = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);

            var fromEmail = new MailAddress("BugTrackerTest12@gmail.com");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "cazhdkrjmgedcqll";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {

                subject = "Your account is successfully created!";
                body = "<br/><br/> We are excited to tell you that your BugTracker account is " +
                                " successfully created! Please click on the below link to verify your account" +
                                "<br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi, <br/>br/>We got a request to reset your account, please click on the below link to reset your password." +
                    "<br/><br/><a href='" + link + ">Reset Password Link</a> ";
            }
            var smtp = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

    }
}