using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Utility;
using Model;
using Model.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminCenter.Controllers
{
    public class AccountController : BaseController
    {
        //登录视图
        public IActionResult Login()
        {
            return View();
        }

        //登录视图
        [HttpPost]
        public IActionResult LoginValidate()
        {
            string account = Request.Form["account"];
            if(string.IsNullOrEmpty(account)){
                return Content(ReturnError("请输入登录账号"));
            }
            string password = Request.Form["password"];
            if (string.IsNullOrEmpty(password))
            {
                return Content(ReturnError("请输入登录密码"));
            }
            string vcode = Request.Form["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(ReturnError("请输入登录验证码"));
            }
            //验证验证码
            var serverCode = HttpContext.Session.GetString("code");
            if(serverCode==null){
                return Content(ReturnError("请输入登录验证码"));
            }
            int result = string.Compare(serverCode, vcode, true);
            if(result>0){
                return Content(ReturnError("验证码错误"));
            }

            var userFromStorage = DB.Queryable<SysUser>().Single(f => f.UserName == account && f.UserPassword == password);  
            if (userFromStorage!=null)    //存在则登录成功, 写入Cookie  
            {  

                return RedirectToAction("Index", "Home"); 
            }  
            else  
            {  
                return Content(ReturnError("帐号或密码错误")); 
            }  
        }

        //注册视图
        public IActionResult Register()
        {
            return View();
        }

        //注册视图
        [HttpPost]
        public IActionResult UserRegister()
        {
            return View();
        }

        //忘记密码视图
        public IActionResult FindPassword()
        {
            return View();
        }

        /// <summary>
        /// 图形验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult ValidateCode()
        {
            string code = "";
            System.IO.MemoryStream ms = Captcha.Create(out code);
            HttpContext.Session.SetString("code", code);
            Response.Body.Dispose();
            return File(ms.ToArray(), @"image/png");
        }
    }
}
