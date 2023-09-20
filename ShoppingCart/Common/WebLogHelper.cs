using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShoppingCart.Common
{
    public class WebLogHelper
    {
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public static void Error(string logStr)
        {
            logger.Error(DoEncode(logStr));
        }
        public static void Error(string logStr, params object[] args)
        {
            logger.Info(DoEncode(logStr), args);
        }

        public static string DoEncode(string logStr)
        {
            string output = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(logStr))
                {
                    output = Regex.Replace(HttpUtility.HtmlEncode(logStr), @"[\r\n]|%0[ad]", string.Empty, RegexOptions.IgnoreCase);
                }
            }
            catch (Exception ex)
            {
                WebLogHelper.Error("DoEncode | {@value}", new { stackTrace = ex.StackTrace });
            }
            return output;
        }
    }    
}