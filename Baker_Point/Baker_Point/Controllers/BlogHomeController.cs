using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baker_Point.Models;

namespace Baker_Point.Controllers
{
    public class BlogHomeController : Controller
    {
        
        public ActionResult Index()
        {
            BPDbContext db = new BPDbContext();
            var blogs=db.Blogs.OrderByDescending(b=>b.BlogsId).ToList();
            List<PushModel> push = new List<PushModel>();
            if (blogs.Count() < 4)
            {
                foreach (var item in blogs)
                {
                    PushModel pm = new PushModel { blog = item };
                    if (item.Context.Count() > 220)
                    {
                        pm.shortcontext = item.Context.Substring(0, 220) + "...";
                    }
                    else
                    {
                        pm.shortcontext = item.Context;
                    }
                    push.Add(pm);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    var item = blogs[i];
                    PushModel pm = new PushModel { blog = item };
                    if (item.Context.Count() > 220)
                    {
                        pm.shortcontext = item.Context.Substring(0, 220) + "...";
                    }
                    else
                    {
                        pm.shortcontext = item.Context;
                    }
                    push.Add(pm);
                }
            }

            return View(push);
        }
    }
}
