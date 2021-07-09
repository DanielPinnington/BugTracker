using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Schema;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        
        // GET: Ticket
        public ActionResult GetTicketsIndex(int page = 1, string sort = "Priorities", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetTickets(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }
        public List<Ticket> GetTickets(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
            {
                using(BugTracking dc = new BugTracking())
                {
                    var v = (from a in dc.Tickets
                             where
                             a.Title.Contains(search) ||
                             a.Description.Contains(search) ||
                             //date should be here will add later
                             a.CurrentUser.Contains(search) ||
                             a.Priorities.Contains(search) //Might comment this ou later
                             
                             select a
                             );
                using (BugTracking dp = new BugTracking())
                {
                    var vv = (from b in dc.Tickets
                              select new Ticket
                              {
                                  Title = b.Title,
                                  Description = b.Description,
                                  Priorities = b.Priorities
                              });
                }

                    totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                


                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }

        public ActionResult saveuser(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";

            //Update data to database 
            using (BugTracking dc = new BugTracking())
            {
                var user = dc.Tickets.Find(id);
                if (user != null)
                {
                    dc.Entry(user).Property(propertyName).CurrentValue = value;
                    dc.SaveChanges();
                    status = true;
                }
                else
                {
                    message = "Error!";
                }
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());
        }

    }
}