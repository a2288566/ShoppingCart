using ShoppingCart.Common;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Service
{
    public class ProductMessageService
    {
        public bool InsertMessage(ProductMessage message)
        {
            bool isSuccess = false;
            ProductMessageRepo result = new ProductMessageRepo();
            try
            {
                isSuccess = result.InsertMessage(message);
            }
            catch (Exception ex)
            {
                WebLogHelper.Error(ex.Message.ToString());
            }
            return isSuccess;
        }
    }
}