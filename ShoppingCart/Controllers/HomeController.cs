using Microsoft.Ajax.Utilities;
using ShoppingCart.Models;
using ShoppingCart.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private ShoppingCartEntities _db;
        private static readonly object _lockObject = new object();
        public HomeController()
        {
            _db = new ShoppingCartEntities();
        }

        public ActionResult Index()
        {
            var product = _db.Product.OrderByDescending(m => m.Id).ToList();
            return View(product);
        }

        [Authorize]
        public ActionResult PushProducts()
        {

            return View();
        }

        [Authorize]
        public ActionResult MyPushProducts()
        {
            var memberID = (int)Session["memberID"];
            var product = _db.Product.Where(x => x.MemberID == memberID).ToList();
            return View(product);
        }

        [HttpPost]
        public JsonResult DoPushProduct(Product product)
        {
            Resp resp = new Resp() { IsSuccess = false };
            product.MemberID = (int)Session["memberID"];

            //瀏覽紀錄待完成
            product.ViewCount = 0;
            ProductsService service = new ProductsService();
            if (!service.InsertProductInfo(product))
            {
                return Json(resp);
            }

            resp.IsSuccess = true;
            return Json(resp);
        }

        [Authorize]
        public ActionResult Edit()
        {
            var product = Session["ProductToEdit"] as Product;
            return View(product);
        }

        public JsonResult DeleteMyProduct(int productId)
        {
            Resp resp = new Resp() { IsSuccess = false };

            var product = _db.Product.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                _db.Product.Remove(product);
                _db.SaveChanges();
                resp.IsSuccess = true;
            }
            return Json(resp);
        }

        [HttpPost]
        public JsonResult GetEditProduct(int productId)
        {
            Resp resp = new Resp() { IsSuccess = false };
            var product = _db.Product.FirstOrDefault(x => x.Id == productId);
            Session["ProductToEdit"] = product;
            resp.IsSuccess = true;

            return Json(resp);
        }

        public ActionResult ProductDetail(int productId)
        {
            lock (_lockObject)
            {
                var product = _db.Product.FirstOrDefault(x => x.Id == productId);
                if (product != null)
                {
                    product.ViewCount++;
                    _db.SaveChanges();
                }
                return View(product);
            }
        }

        [HttpPost]
        public JsonResult EditProduct(Product product)
        {
            Resp resp = new Resp() { IsSuccess = false };
            var productToEdit = _db.Product.FirstOrDefault(x => x.Id == product.Id);
            try
            {
                productToEdit.ProductTitle = product.ProductTitle;
                productToEdit.Classification = product.Classification;
                productToEdit.Price = product.Price;
                _db.SaveChanges();
                resp.IsSuccess = true;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
            }

            return Json(resp);
        }

        [Authorize]
        public ActionResult Cart()
        {
            int memberID = (int)Session["memberID"];
            var cartProduct = _db.CartSet.FirstOrDefault(x => x.MemberID == memberID);

            if (cartProduct == null)
                return View();
            else
            {
                var query = from c in _db.CartSet
                            join p in _db.Product on c.ProductID equals p.Id into gj
                            from subP in gj.DefaultIfEmpty()
                            where c.MemberID == memberID
                            select new CartModel
                            {
                                CartID = c.CartID,
                                ProductID = subP.Id,
                                ProductTitle = subP.ProductTitle,
                                Price = subP.Price,
                                Quantity = c.Quantity,
                                TotalPrice = c.TotalPrice
                            };

                var result = query.ToList();
                return View(result);
            }

        }

        [HttpPost]
        public JsonResult AddShoppingCart(CartSet cartProduct)
        {
            Resp resp = new Resp() { IsSuccess = false };

            //判斷商品是否為自己的
            var product = _db.Product.FirstOrDefault(x => x.Id == cartProduct.ProductID);
            if (product.MemberID == (int)Session["memberID"])
            {
                resp.Message = "無法將自己的商品加入購物車";
                return Json(resp);
            }

            //判斷購物車是否有相同物件
            var checkDuplicateProduct = _db.CartSet.FirstOrDefault(x => x.ProductID == cartProduct.ProductID);
            if (checkDuplicateProduct != null)
            {
                int productAddCartQuantity = cartProduct.Quantity;
                int productPrice = Int32.Parse(product.Price);
                checkDuplicateProduct.Quantity += productAddCartQuantity;
                checkDuplicateProduct.TotalPrice = (checkDuplicateProduct.Quantity * productPrice).ToString();
                _db.SaveChanges();
            }
            else
            {
                //加入購物車
                cartProduct.MemberID = (int)Session["memberID"];
                CartService service = new CartService();
                if (!service.InsertProductToCart(cartProduct))
                {
                    resp.Message = "加入購物車錯誤";
                    return Json(resp);
                }
            }

            resp.IsSuccess = true;
            return Json(resp);
        }

        public JsonResult DeleteMyCartProduct(int cartId)
        {
            Resp resp = new Resp() { IsSuccess = false };

            var removeCartProduct = _db.CartSet.FirstOrDefault(x => x.CartID == cartId);
            if (removeCartProduct != null)
            {
                _db.CartSet.Remove(removeCartProduct);
                _db.SaveChanges();
                resp.IsSuccess = true;
            }
            return Json(resp);
        }

        [HttpPost]
        public JsonResult GetMessage(int productID)
        {
            MessageResp resp = new MessageResp() { IsSuccess = false };

            var getMessage = _db.ProductMessage.Where(x => x.ProductID == productID).ToList();
            resp.messageList = getMessage;
            resp.IsSuccess = true;
            return Json(resp);
        }

        [HttpPost]
        public JsonResult InsertMessage(ProductMessage productMessage)
        {
            Resp resp = new Resp() { IsSuccess = false };

            productMessage.CreateDate = DateTime.Now;
            ProductMessageService service = new ProductMessageService();
            if (!service.InsertMessage(productMessage))
            {
                resp.Message = "加入留言錯誤";
                return Json(resp);
            }

            resp.IsSuccess = true;
            return Json(resp);
        }
    }
}