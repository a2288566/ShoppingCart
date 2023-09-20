using ShoppingCart.Common;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ShoppingCart.Repository
{
    public class CartRepo
    {
        public bool InsertProductToCart(CartSet cartProduct)
        {
            using (ShoppingCartEntities db = new ShoppingCartEntities())
            {
                try
                {
                    db.CartSet.Add(cartProduct);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationResult in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationResult.ValidationErrors)
                        {
                            //resp.Message += ($"實體 {entityValidationResult.Entry.Entity.GetType().Name} 驗證失敗：{validationError.ErrorMessage}");
                            WebLogHelper.Error($"{MethodBase.GetCurrentMethod().Name} | {{@value}}", new
                            {
                                message = validationError.ErrorMessage,
                                exception = validationError.ErrorMessage.ToString()
                            });
                        }
                    }
                    return false;
                }
            }

            return true;
        }
    }
}