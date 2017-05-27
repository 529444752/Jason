using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baker_Point.Models
{
    public class ProductList
    {
        public int ProductListId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Number { get; set; }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public virtual UserProfile Costomer { get; set; }
    }

    public class BPDbContext:DbContext
    { 
        public BPDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductList> pList { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<rootCart> Cart { get; set; }
        public DbSet<lastView> lastViewedProduct { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserAvatar> Avatars { get; set; }
        public DbSet<FeaturedList> Featured { get; set; }
    }
}