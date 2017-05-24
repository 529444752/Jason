using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baker_Point.Models;
using System.Xml;
using System.IO;

namespace Baker_Point.Controllers
{
    public class ProductController : Controller
    {
        private BPDbContext db = new BPDbContext();

        public class GetCartModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Number { get; set; }
            public int Price { get; set; }
        }
        //
        // GET: /Product/
        public void ReWriteCart()
        {
            int user = getCurrentUser();
            var cart = db.Cart.Where(u => u.UserId == user).ToList();
            int num = 0;
            if (cart.Count != 0)
            {
                int next = cart[0].next;
                while (next != 0)
                {
                    var a = db.CartItems.Find(next);
                    num += a.Number;                    
                    next = a.next;
                }
            }
        }

        public List<GetCartModel> returnCart()
        {
            int userid = getCurrentUser();
            List<GetCartModel> CartItemList = new List<GetCartModel>();
            int next = db.Cart.First(c => c.UserId == userid).next;
            while (next != 0)
            {
                CartItem item = db.CartItems.Find(next);
                GetCartModel model = new GetCartModel();
                model.ProductId = item.ProductId;
                model.Number = item.Number;
                Product product = db.Products.Find(item.ProductId);
                model.Price = product.Price;
                model.ProductName = product.ProductName;
                CartItemList.Add(model);
                next = item.next;
            }
            return CartItemList;
        }

        public int getCartproduct(int targetId)
        {
            int CartItemId = 0;
            int User = getCurrentUser();
            int next = 0;
            var cart = db.Cart.Where(c => c.UserId == User).ToList();
            if (cart.Count != 0)
            {
                next = cart.ToList()[0].next;
            }

            while (next != 0)
            {
                var a = db.CartItems.Find(next);
                if (a.ProductId == targetId)
                {
                    CartItemId = a.CartItemId;
                    break;
                }
                next = a.next;
            }
            return CartItemId;
        }

        public int getCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var get = db.UserProfiles.Where(m => m.UserName == User.Identity.Name);
                return (get.ToList().ElementAt(0).UserId);
            }
            else
                return 0;
        }

        public void add(int id)
        {
            CartItem item = db.CartItems.Find(id);
            item.Number++;
            MvcApplication.CartItemNUM++;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            ReWriteCart();
        }

        public void subtract(int id)
        {
            try
            {
                CartItem item = db.CartItems.Find(id);
                if (item.Number > 1)
                {
                    item.Number--;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var list = db.CartItems.Where(p => p.next == id).ToList();
                    if (list.Count != 0)
                    {
                        CartItem pre = list.ElementAt(0);
                        pre.next = item.next;
                        db.CartItems.Remove(item);
                        db.Entry(pre).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        var root = db.Cart.Where(c => c.next == id).ToList();
                        rootCart cart = root.ElementAt(0);
                        cart.next = item.next;
                        db.CartItems.Remove(item);
                        db.Entry(cart).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                MvcApplication.CartItemNUM--;
            }
            catch (Exception)
            {
                return;
            }
            ReWriteCart();
        }

        public void bindCart()
        {
            int userid = getCurrentUser();
            if (db.Cart.Where(c => c.UserId == userid).Count() == 0)
            {
                rootCart rc = new rootCart();
                rc.UserId = userid;
                rc.next = 0;
                db.Cart.Add(rc);
                db.SaveChanges();
            }
        }


        public void setLastView(int id)
        {
            int userid = getCurrentUser();
            if (db.lastViewedProduct.Where(p => p.UserID == userid).Count() == 0)
            {
                lastView lv = new lastView();
                lv.UserID = userid;
                lv.ProductID = id;
                db.lastViewedProduct.Add(lv);
                db.SaveChanges();
            }
            else
            {
                var lastview = db.lastViewedProduct.First(p => p.UserID == userid);
                lastview.ProductID = id;
                db.Entry(lastview).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        [Authorize]
        public ActionResult Index(int CategoryId=0,string productName=null)
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");
            ViewBag.empty = 1;
            List<Product> productlist = new List<Product>();
            var products = db.Products.Include(p => p.Category).ToList();
            if (productName != null)
            {
                var a = products.Where(m => m.ProductName == productName).ToList();
                productlist.Concat(a);
                products.Except(a);
                if (productName.Length > 1)
                {
                    for (int i = 0; i < productName.Length-1; i++)
                    {
                        int num = 0;
                        string key1 = productName[i].ToString().ToUpper() + productName[i + 1].ToString().ToLower();
                        string key2 = productName[i].ToString().ToUpper() + productName[i + 1].ToString().ToUpper();
                        string key3 = productName[i].ToString().ToLower() + productName[i + 1].ToString().ToLower();
                        string key4 = productName[i].ToString().ToLower() + productName[i + 1].ToString().ToUpper();
                        for (int j = 0; j < products.Count; j++)
                        {
                            var p = products.ElementAt(num);
                            var n = productName[i];
                            if (p.ProductName.Contains(key1) || p.ProductName.Contains(key2) || p.ProductName.Contains(key3) || p.ProductName.Contains(key4))
                            {
                                productlist.Add(p);
                                products.Remove(p);
                            }
                            else
                                num++;
                        }
                    }
                }
            }
            else
            {
                productlist = products;
            }
            if (CategoryId!=0)
            {
                productlist = productlist.Where(p => p.CategoryID == CategoryId).ToList();
                ViewBag.Category = db.Categories.First(p => p.CategoryID == CategoryId).Description;
            }
            else
            {
                ViewBag.Category = "All Categories";
            }
            if (productlist.Count == 0)
                ViewBag.empty = 0;
            ViewBag.Categories = new SelectList(db.Categories,"CategoryId","Description",CategoryId);
            return View(productlist);
        }

        [HttpPost]
        public ActionResult Index(List<Product> products, int Category)
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");
            ViewBag.empty = 1;
            if (products != null)
            {
                if (Category != 0)
                {
                    products = products.Where(p => p.CategoryID == Category).ToList();
                    if (products.Count == 0)
                        ViewBag.empty = 0;
                }
                else
                    products = db.Products.ToList();
                ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "Description", Category);
            }
            else
            {
                products = db.Products.ToList();
                ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "Description",String.Empty);
            }
            return View(products);
        }

        //
        // GET: /Product/Details/5


        [Authorize]
        public ActionResult Cart()
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");
            bindCart();
            List<GetCartModel> list = returnCart();
            if (list.Count == 0)
            {
                ViewBag.empty = false;
            }
            else
            {
                ViewBag.empty = true;
            }
            return View(list);             
        }

        [HttpPost]
        public ActionResult Cart(int id = 0, int mode = 2)
        {
            if (mode == 0)
            {
                subtract(getCartproduct(id));
            }
            else if (mode == 1)
            {
                add(getCartproduct(id));
            }
            List<GetCartModel> list = returnCart();
            if (list.Count == 0)
            {
                ViewBag.empty = false;
            }
            else
            {
                ViewBag.empty = true;
            }
            return RedirectToAction("Cart");    
        }

        [Authorize]
        public ActionResult Details(string purchase,int id = 0)
        {
            bindCart();
            setLastView(id);
            if (purchase!=null)
            {
                int CartId = getCartproduct(id);
                if (CartId == 0)
                {
                    int CurrentUser = getCurrentUser();
                    rootCart cart = db.Cart.Where(m => m.UserId == CurrentUser).ToList().ElementAt(0);
                    CartItem ci = new CartItem();
                    ci.ProductId = id;
                    ci.Number = 1;
                    ci.next = cart.next;
                    db.CartItems.Add(ci);
                    db.SaveChanges();                    
                    cart.next = ci.CartItemId;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();
                    ReWriteCart();
                }
                else
                {
                    var cartitem = db.CartItems.Find(CartId);
                    cartitem.Number++;
                    db.Entry(cartitem).State = EntityState.Modified;
                    db.SaveChanges();
                    ReWriteCart();
                }
                MvcApplication.CartItemNUM++;
            }
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        [Authorize(Roles="admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description");
            return View();
        }

        //
        // POST: /Product/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Product product)
        {
            
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                string path = "/Images/Products/" + product.ProductID + ".jpg";
                string filepath = Server.MapPath(path);
                HttpPostedFileBase hpf = Request.Files["upload"];
                hpf.SaveAs(filepath);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", product.CategoryID);
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        [Authorize(Roles="admin")]
        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", product.CategoryID);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", product.CategoryID);
            return View(product);
        }

        //
        // GET: /Product/Delete/5
        [Authorize(Roles="admin")]
        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}