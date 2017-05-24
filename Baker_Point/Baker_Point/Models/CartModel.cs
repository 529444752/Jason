using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Baker_Point.Models
{
    public class rootCart
    {
        public int rootCartId { get; set; }
        public int UserId { get; set; }
        public int next { get; set; }
    }

    public class CartItem
    {
        public int CartItemId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual Product product { get; set; }
        [Required]
        public int Number { get; set; }
        public int next { get; set; }
    }
}