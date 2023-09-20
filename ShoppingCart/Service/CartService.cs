using ShoppingCart.Common;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Service
{
    public class CartService
    {
        public bool InsertProductToCart(CartSet cartProduct)
        {
            bool isSuccess = false;
            CartRepo result = new CartRepo();
            try
            {
                isSuccess = result.InsertProductToCart(cartProduct);
            }
            catch (Exception ex)
            {
                WebLogHelper.Error(ex.Message.ToString());
            }
            return isSuccess;
        }
    }
}