using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Baker_Point.Models;
using Baker_Point.Filters;
using System.Xml;

namespace Baker_Point.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {

        private BPDbContext db = new BPDbContext();

        public class OrderItem
        {
            public int id { get; set; }
            public int Number { get; set; }
            public int Price { get; set; }
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

        public void ReWriteXml(List<Product> list)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlNode rootNode = xmlDoc.CreateElement("root");
            foreach (var product in list)
            {
                XmlNode item = xmlDoc.CreateElement("ProductID");
                XmlAttribute itemattr = xmlDoc.CreateAttribute("ID");
                XmlAttribute itemname = xmlDoc.CreateAttribute("Name");
                itemattr.Value = product.ProductID.ToString();
                itemname.Value = product.ProductName.ToString();
                item.Attributes.Append(itemattr);
                item.Attributes.Append(itemname);
                rootNode.AppendChild(item);
            }
            //append root node
            xmlDoc.AppendChild(rootNode);

            xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);
            xmlDoc.Save(Server.MapPath("//ProductId.xml"));
        }

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
            MvcApplication.CartItemNUM = num;
        }

        public ActionResult Index()
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");

            ReWriteCart();
            List<OrderItem> list = new List<OrderItem>();
            List<Product> recommendList = new List<Product>();
            if (User.Identity.IsAuthenticated)
            {
                int id = getCurrentUser();
                var orders = db.Orders.Where(o => o.UserID == id).ToList();
                if (orders.Count == 0)
                {
                    ViewBag.Order = null;
                }
                else
                {                    
                    var LatestOrder = orders[orders.Count - 1];
                    IEnumerable<ProductList> pList = from pl in db.pList let k = pl.OrderId where LatestOrder.OrderID.Equals(k) select pl;
                    var LpList = pList.ToList();
                    foreach (var pitem in LpList)
                    {
                        OrderItem item = new OrderItem();
                        IEnumerable<Product> p = from product in db.Products let k = product.ProductID where pitem.ProductId.Equals(k) select product;
                        item.id = pitem.ProductId;
                        item.Number = pitem.Number;
                        item.Price = p.ElementAt(0).Price;
                        list.Add(item);
                    }
                    ViewBag.Order = list;
                }
                var lastview = db.lastViewedProduct.Where(m => m.UserID == id).ToList();
                int Category = 0;

                if (lastview.Count == 0)
                {
                    ViewBag.LastView = null;
                }
                else
                {
                    int pid=lastview[0].ProductID;
                    ViewBag.LastView = db.lastViewedProduct.Include("lastViewed").Where(m => m.UserID == id).ToList();
                    Category = db.lastViewedProduct.Include("lastViewed").Where(m => m.UserID == id).ToList()[0].lastViewed.CategoryID;
                    recommendList = db.Products.Where(p => p.CategoryID == Category && p.ProductID != pid).ToList();
                }

            }
            List<Product> products=db.Products.ToList();
            products.Reverse();
            ReWriteXml(products.Take(4).ToList());
            return View(recommendList);
        }

        public ActionResult About()
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");

            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            Response.Write("<script type='text/javascript'>window.onload=function(){var c = document.getElementById('cartnum');var num = " + MvcApplication.CartItemNUM + "+'';var cxt = c.getContext('2d');cxt.fillStyle = '#ff0000';cxt.beginPath();cxt.arc(10, 10, 10, 0, Math.PI * 2, true);cxt.closePath();cxt.fill();cxt.font = '10px Arial';cxt.textAlign = 'center';cxt.textBaseline = 'middle';cxt.fillStyle = 'white';cxt.fillText(num, 10, 10, 15);}</script>");

            ViewBag.Message = "你的联系方式页。";

            return View();
        }
    }
}
