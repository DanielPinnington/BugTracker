using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Schema;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        
        // GET: Ticket
        public ActionResult GetTicketsIndex(int page = 1, string sort = "Title", string sortdir = "asc", string search = "")
        {
            int totalRecord =0;
            int pageSize = 10;
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
                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }
    }
}