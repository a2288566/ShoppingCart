using Microsoft.Ajax.Utilities;
using ShoppingCart.Common;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ShoppingCart.Service
{
    public class LoginService
    {
        public bool InsertUserInfo(MemberSet member)
        {
            bool isSuccess = false;
            LoginRepo result = new LoginRepo();
            try
            {
                isSuccess = result.InsertUserInfo(member);
            }
            catch (Exception ex)
            {
                WebLogHelper.Error(ex.Message.ToString());
            }
            return isSuccess;
        }
    }
}