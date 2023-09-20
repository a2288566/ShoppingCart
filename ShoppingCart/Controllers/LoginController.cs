using Microsoft.Ajax.Utilities;
using ShoppingCart.Models;
using ShoppingCart.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using ShoppingCart.Common;

namespace ShoppingCart.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.account = (string)Session["RememberAccount"];
            ViewBag.checkRemember = (string)Session["CheckRemember"];
            Session.RemoveAll();
            return View();
        }

        [HttpPost]
        public JsonResult Login(MemberSet member)
        {
            Resp resp = new Resp() { IsSuccess = false };

            //驗證資訊
            var db = new ShoppingCartEntities();
            var checkMember = db.MemberSet.Where(x => x.Account == member.Account).ToList().FirstOrDefault();
            if (checkMember == null)
            {
                resp.Message = "查無此帳號";
                return Json(resp);
            }
            if (member.Password != HashHelper.Decrypt(checkMember.Password))
            {
                resp.Message = "密碼錯誤";
                return Json(resp);
            }
            Session["userName"] = checkMember.Name;
            Session["memberID"] = checkMember.Id;

            //記住登入資訊
            FormsAuthentication.SetAuthCookie(member.Name, false);

            resp.IsSuccess = true;
            return Json(resp);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(MemberSet member)
        {
            Resp resp = new Resp() { IsSuccess = false };

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                resp.Message = errors;
                return Json(resp);
            }

            if (!IsValidPhoneNumber(member.Phone))
            {
                resp.Message = "手機號碼格式錯誤";
                return Json(resp);
            }

            member.Password = HashHelper.Encrypt(member.Password);

            //進DB註冊資料
            LoginService service = new LoginService();
            if (!service.InsertUserInfo(member))
            {
                resp.Message = "註冊失敗";
                return Json(resp);
            }

            resp.IsSuccess = true;
            resp.Message = "註冊成功";
            return Json(resp);
        }

        public bool IsValidPhoneNumber(string phone)
        {
            if (phone.Length != 10 || phone.Substring(0,2) != "09")
            {
                return false;
            }
            // 定義只含數字的正則表達式
            string pattern = @"^\d+$";

            // 使用正則表達式進行驗證
            return Regex.IsMatch(phone, pattern);
        }

        [HttpPost]
        public JsonResult RememberAccount(MemberSet member)
        {
            Resp resp = new Resp() { IsSuccess = false };
            Session["RememberAccount"] = member.Account;
            Session["CheckRemember"] = "on";
            return Json(resp);
        }

        public ActionResult Logout()
        {
            //保存記住帳號
            var rememberAccount = Session["RememberAccount"];
            var checkRemember = Session["CheckRemember"];
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session["RememberAccount"] = rememberAccount;
            Session["CheckRemember"] = checkRemember;

            return RedirectToAction("index", "Home");
        }
    }
}