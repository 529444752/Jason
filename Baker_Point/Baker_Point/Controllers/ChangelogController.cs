using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baker_Point.Models;
using Baker_Point.Filters;

namespace Baker_Point.Controllers
{
    [Authorize(Roles ="Admin")]
    [InitializeSimpleMembership]
    public class ChangelogController : Controller
    {
        private BPDbContext db = new BPDbContext();

        //
        // GET: /Changelog/

        public ActionResult Index()
        {
            return View(db.Changelogs.ToList());
        }
    

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}