using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Baker_Point.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int Price { get; set; }
        public virtual Category Category { get; set; }
    }

    public class lastView
    {
        public int lastViewID { get; set; }
        public int UserID { get; set; }
        public virtual UserProfile User { get; set; }
        public int ProductID { get; set; }
        public virtual Product lastViewed { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string Description { get; set; }
    }
}