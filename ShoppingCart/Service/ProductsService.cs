using ShoppingCart.Common;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Service
{
    public class ProductsService
    {
        public bool InsertProductInfo(Product product)
        {
            bool isSuccess = false;
            ProductRepo result = new ProductRepo();
            try
            {
                isSuccess = result.InsertProductInfo(product);
            }
            catch (Exception ex)
            {
                WebLogHelper.Error(ex.Message.ToString());
            }
            return isSuccess;
        }
    }
}