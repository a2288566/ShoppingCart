using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class CartModel
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }
    }
}