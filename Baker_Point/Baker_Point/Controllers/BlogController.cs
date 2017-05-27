using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Baker_Point.Models;
using Baker_Point.Filters;

namespace Baker_Point.Controllers
{
    [InitializeSimpleMembership]
    public class BlogController : Controller
    {
        BPDbContext db = new BPDbContext();

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        //
        // GET: /Blog/Details/5

        public ActionResult Details(int id = 0)
        {
            Blogs blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.Comments = db.Comments.Where(m => m.BlogsId == id).ToList();
            return View(blog);
        }

        //
        // GET: /Blog/Create

        [Authorize]
        public ActionResult Create()
        {
            Blogs blog = new Blogs { BlogTitle = "", Context = "" };
            return View(blog);
        }

        //
        // POST: /Blog/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blogs blog)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files["image-file"].FileName != "")
                {
                    HttpPostedFileBase NewPic = Request.Files["image-file"];
                    string extend = NewPic.FileName.Substring(NewPic.FileName.LastIndexOf("."));
                    string Path = "/Images/Title/" + Guid.NewGuid() + extend;
                    NewPic.SaveAs(Server.MapPath(Path));
                    blog.titleImg = Path;
                }
                else
                {
                    blog.titleImg = "/Images/img/4.jpg";
                }
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        //
        // GET: /Blog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Blogs blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        //
        // POST: /Blog/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blogs blog)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files["image-file"].FileName != "")
                {
                    FileInfo Fi = new FileInfo(Server.MapPath(blog.titleImg));
                    Fi.Delete();
                    HttpPostedFileBase NewPic = Request.Files["image-file"];
                    string extend = NewPic.FileName.Substring(NewPic.FileName.LastIndexOf("."));
                    string Path = "/Images/Title/" + Guid.NewGuid() + extend;
                    NewPic.SaveAs(Server.MapPath(Path));

                    blog.titleImg = Path;
                }
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        //
        // GET: /Blog/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Blogs blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            if (WebSecurity.CurrentUserId != blog.UserId ||User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        //
        // POST: /Blog/Delete/5

        [HttpPost, ActionName("Delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
            Blogs blog = db.Blogs.Find(id);
            var comments = db.Comments.Where(c => c.BlogsId == id);
            foreach (var item in comments)
            {
                db.Comments.Remove(item);
            }
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comment comment)
        {
            Blogs blog = db.Blogs.Find(comment.BlogsId);
            blog.Comments++;
            db.Entry(blog);
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = comment.BlogsId });
        }

        [HttpPost]
        public ActionResult FeatureBlog(int id)
        {
            Blogs blog = db.Blogs.Find(id);
            var list = db.Featured.ToList();
            if (list.Count < 6)
            {
                FeaturedList fl = new FeaturedList { BlogsId = blog.BlogsId };
                db.Featured.Add(fl);
            }
            else
            {
                FeaturedList fl = list[0];
                fl.blog.IsFeatured = false;
                FeaturedList newfl = new FeaturedList { BlogsId = blog.BlogsId };
                db.Entry(fl.blog);
                db.Featured.Remove(fl);
                db.Featured.Add(newfl);
            }
            blog.IsFeatured = true;
            db.Entry(blog);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id, int blogid)
        {
            Comment comment = db.Comments.Find(id);
            Blogs blog = db.Blogs.Find(blogid);
            blog.Comments--;
            db.Entry(blog).State = EntityState.Modified;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = blogid });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}