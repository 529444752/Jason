using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baker_Point.Models;
using System.Xml;

namespace Baker_Point.Controllers
{
    public class OrderController : Controller
    {
        private BPDbContext db = new BPDbContext();

        //
        // GET: /Order/

        public class GetCartModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Number { get; set; }
            public int Price { get; set; }
        }
        //
        // GET: /Product/
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

        public void clearCart()
        {
            int userid = getCurrentUser();
            var user = db.Cart.First(c => c.UserId == userid);
            int next = user.next;
            while (next != 0)
            {
                CartItem item = db.CartItems.Find(next);
                next = item.next;
                db.CartItems.Remove(item);
            }
            user.next = 0;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public class OrderItem
        {
            public int id{get;set;}
            public int Number{get;set;}
            public int Price{get;set;}
        }

        public class OrderIndexModel
        {
            public int id { get; set; }
            public string description { get; set; }
        }

        public class OrderIndex
        {
            public int orderid { get; set; }
            public List<OrderIndexModel> oim { get; set; }
        }

        public class OrderEditModel
        {
            public int OrderId { get; set; }
            public List<OrderItem> OrderitemList { get; set; }
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

        [Authorize]
        public ActionResult Index()
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");

            List<OrderIndex> oil = new List<OrderIndex>();
            if (!User.IsInRole("Admin"))
            {
                int id = getCurrentUser();
                var orders = db.Orders.Where(o => o.UserID == id).ToList();
                foreach (var item in orders)
                {
                    List<OrderIndexModel> list = new List<OrderIndexModel>();
                    OrderIndex oi = new OrderIndex();
                    IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where item.OrderID.Equals(k) select pl;
                    var LpList = pList.ToList();
                    foreach (var pitem in LpList)
                    {
                        OrderIndexModel oim = new OrderIndexModel();
                        IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                        oim.id = p.ElementAt(0).ProductID;
                        oim.description = p.ElementAt(0).ProductName + "    *" + pitem.Number;
                        list.Add(oim);
                    }
                    oi.orderid = item.OrderID;
                    oi.oim = list;
                    oil.Add(oi);
                }
                ViewBag.index = oil;
            }
            else
            {
                var orders = db.Orders.ToList();
                foreach (var item in orders)
                {
                    List<OrderIndexModel> list = new List<OrderIndexModel>();
                    OrderIndex oi = new OrderIndex();
                    IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where item.OrderID.Equals(k) select pl;
                    var LpList = pList.ToList();
                    foreach (var pitem in LpList)
                    {
                        OrderIndexModel oim = new OrderIndexModel();
                        IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                        oim.id = p.ElementAt(0).ProductID;
                        oim.description = p.ElementAt(0).ProductName + "    *" + pitem.Number;
                        list.Add(oim);
                    }
                    oi.orderid = item.OrderID;
                    oi.oim = list;
                    oil.Add(oi);
                }
                ViewBag.index = oil;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            List<OrderIndex> oil = new List<OrderIndex>();            
            var User = db.UserProfiles.Where(m => m.UserName == name).ToList();
            if (User.Count() != 0)
            {
                var id = User[0].UserId;
                var orders = db.Orders.Where(o => o.UserID == id).ToList();
                foreach (var item in orders)
                {
                    List<OrderIndexModel> list = new List<OrderIndexModel>();
                    OrderIndex oi = new OrderIndex();
                    IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where item.OrderID.Equals(k) select pl;
                    var LpList = pList.ToList();
                    foreach (var pitem in LpList)
                    {
                        OrderIndexModel oim = new OrderIndexModel();
                        IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                        oim.id = p.ElementAt(0).ProductID;
                        oim.description = p.ElementAt(0).ProductName + "    *" + pitem.Number;
                        list.Add(oim);
                    }
                    oi.orderid = item.OrderID;
                    oi.oim = list;
                    oil.Add(oi);
                }
            }
            ViewBag.index = oil;

            return View();
        }
        //
        // GET: /Order/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                int userid = getCurrentUser();
                if (User.IsInRole("admin") || order.UserID == userid)
                {
                    var username = db.UserProfiles.Find(order.UserID).UserName;
                    ViewBag.UserName = username;
                    OrderIndex oi = new OrderIndex();
                    List<OrderIndexModel> list = new List<OrderIndexModel>();
                    IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where order.OrderID.Equals(k) select pl;
                    var LpList = pList.ToList();
                    foreach (var pitem in LpList)
                    {
                        OrderIndexModel oim = new OrderIndexModel();
                        IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                        oim.id = p.ElementAt(0).ProductID;
                        oim.description = p.ElementAt(0).ProductName + "    *" + pitem.Number;
                        list.Add(oim);
                    }
                    oi.orderid = id;
                    oi.oim = list;
                    return View(oi);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            List<GetCartModel> modelList = returnCart();
            return View(modelList);
            /*List<OrderModel> om = new List<OrderModel>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("//Mart.xml"));
            XmlNode root = xmlDoc.SelectSingleNode("root");
            foreach (XmlNode item in root.ChildNodes)
            {
                XmlElement e = (XmlElement)item;
                int pid = Convert.ToInt32(e.GetAttribute("ID"));
                int num = Convert.ToInt32(e.GetAttribute("Num"));
                IQueryable<Product> q = db.Products.Where(a => a.ProductID == pid);
                OrderModel order = new OrderModel();
                order.Number = num;
                order.ProductName = q.ToList().ElementAt(0).ProductName;
                order.Price = q.ToList().ElementAt(0).Price * num;
                om.Add(order);
            }
            ViewBag.Order = om;
            return View();*/
        }

        //
        // POST: /Order/Create

        [HttpPost]
        public ActionResult Create(Order order)
        {
            order = new Order();
            List<GetCartModel> list = returnCart();
            if (ModelState.IsValid&&User.Identity.IsAuthenticated)
            {
                order.UserID = db.UserProfiles.Where(m => m.UserName == User.Identity.Name).ToList().ElementAt(0).UserId;
                db.Orders.Add(order);
                db.SaveChanges();
                foreach (var item in list)
                {
                    int pid = item.ProductId;
                    int num = item.Number;
                    ProductList pl = new ProductList();
                    pl.Number = num;
                    pl.ProductId = pid;
                    pl.OrderId = order.OrderID;
                    db.pList.Add(pl);
                }
                db.SaveChanges();
                clearCart();
                return RedirectToAction("Index");
            }

            return View(list);
        }

        //
        // GET: /Order/Edit/5

        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            OrderEditModel oem = new OrderEditModel();
            List<OrderItem> list = new List<OrderItem>();
            IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where order.OrderID.Equals(k) select pl;
            var LpList = pList.ToList();
            foreach (var pitem in LpList)
            {
                OrderItem oi = new OrderItem();
                IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                oi.id = p.ElementAt(0).ProductID;
                oi.Number = pitem.Number;
                oi.Price = p.ElementAt(0).Price;
                list.Add(oi);
            }
            oem.OrderId = id;
            oem.OrderitemList = list;

            return View(oem);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderEditModel oem)
        {
            if (oem!=null)
            {
                var pList = db.pList.AsNoTracking().Where(p => p.OrderId == oem.OrderId).ToList();
                foreach (var item in oem.OrderitemList)
                {
                    var order = pList.Where(p => p.ProductId == item.id).ToList().ElementAt(0);
                    if (item.Number == 0)
                    {
                        db.pList.Remove(order);
                        pList.Remove(order);
                    }
                    else
                    {
                        ProductList pl = new ProductList();
                        pl.Number = item.Number;
                        pl.OrderId = oem.OrderId;
                        pl.ProductId = item.id;
                        pl.ProductListId = order.ProductListId;
                        db.Entry(pl).State = EntityState.Modified;
                        
                    }
                }
                if (pList.Count() == 0)
                {
                    var order = db.Orders.Find(oem.OrderId);
                    db.Orders.Remove(order);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oem);
        }

        //
        // GET: /Order/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            OrderEditModel oem = new OrderEditModel();
            List<OrderItem> list = new List<OrderItem>();
            IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where order.OrderID.Equals(k) select pl;
            var LpList = pList.ToList();
            foreach (var pitem in LpList)
            {
                OrderItem oi = new OrderItem();
                IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                oi.id = p.ElementAt(0).ProductID;
                oi.Number = pitem.Number;
                oi.Price = p.ElementAt(0).Price;
                list.Add(oi);
            }
            oem.OrderId = id;
            oem.OrderitemList = list;

            return View(oem);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(OrderEditModel oem)
        {
            Order order = db.Orders.Find(oem.OrderId);
            db.Orders.Remove(order);
            var pList = db.pList.Where(p=>p.OrderId==oem.OrderId).ToList();
            foreach (var item in pList)
            {
                db.pList.Remove(item);
            }
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