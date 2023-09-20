using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class MessageResp : Resp
    {
        public List<ProductMessage> messageList { get; set; } = new List<ProductMessage>();
    }
}