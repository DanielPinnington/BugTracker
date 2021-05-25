using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using static DataLibrary.Logic.EmployeeProcessor;
using static DataLibrary.DataAccess.SqlDataAccess;
using System.Configuration;
using DataLibrary.DataAccess;
using System.Data.SqlClient;
using Google.Apis.Admin.Directory.directory_v1.Data;
using BugTracker.Models;
namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ViewEmployees()
        {
            ViewBag.Message = "Employees List";

            var data = LoadEmployees();
            List<EmployeeModel> employees = new List<EmployeeModel>();

            foreach(var row in data)
            {
                employees.Add(new EmployeeModel{
                    EmployeeID = row.EmployeeID,
                    UsernameID = row.UsernameID,
                    FirstName = row.FirstName,
                    SecondName = row.SecondName,
                    EmailAddress = row.EmailAddress
                    
                });
            }

            return View(employees);
        }

        public ActionResult SignUp()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogIn()
        {
            string debugDBConnection = ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(debugDBConnection);
            SqlCommand sqlComm = new SqlCommand("select * from [db].[employee] where Username = @Username and Password = @Password", sqlConn);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(EmployeeModel model)
        {

            if (ModelState.IsValid)
            {
                int recordsCreated = CreateEmployee(model.EmployeeID, model.UsernameID, model.FirstName, model.SecondName, model.EmailAddress);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}