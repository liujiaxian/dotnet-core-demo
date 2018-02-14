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
using Service;
using AdminCenter.Application;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminCenter.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        //登录视图
        public IActionResult Login()
        {
            if (HttpContext.Request.Cookies[ApplicationKeys.User_Cookie_Key] != null)
            {
                return Redirect("/home/index");
            }
            return View();
        }

        //登录逻辑
        [HttpPost]
        public IActionResult LoginValidate()
        {
            string account = Request.Form["account"];
            if (string.IsNullOrEmpty(account))
            {
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
            if (serverCode == null)
            {
                return Content(ReturnError("请输入登录验证码"));
            }
            int result = string.Compare(serverCode, vcode, true);
            if (result > 0)
            {
                return Content(ReturnError("验证码错误"));
            }
            //调用服务验证用户名密码
            if (!_userService.Login(account, password))
            {
                return Content(ReturnError("用户名或密码错误"));
            }
            //加密用户名写入cookie中，AdminAuthorizeAttribute特性标记取出cookie并解码除用户名
            var encryptValue = _userService.LoginEncrypt(account, ApplicationKeys.User_Cookie_Encryption_Key);
            HttpContext.Response.Cookies.Append(ApplicationKeys.User_Cookie_Key, encryptValue);

            //清除验证码
            HttpContext.Session.Remove("code");

            return Content(ReturnSuccess("登录成功"));
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
            string email = Request.Form["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnError("请输入邮箱"));
            }
            string account = Request.Form["account"];
            if (string.IsNullOrEmpty(account))
            {
                return Content(ReturnError("请输入登录账号"));
            }

            //判断账户和邮箱是否存在
            if (_userService.IsExistEmail(email))
            {
                return Content(ReturnError("邮箱已存在"));
            }

            if (_userService.IsExistAccount(account))
            {
                return Content(ReturnError("帐号已存在"));
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
            if (serverCode == null)
            {
                return Content(ReturnError("请输入登录验证码"));
            }
            int result = string.Compare(serverCode, vcode, true);
            if (result > 0)
            {
                return Content(ReturnError("验证码错误"));
            }

            //调用服务验证用户名密码
            if (!_userService.Register(email, account, password))
            {
                return Content(ReturnError("注册失败"));
            }

            //清除验证码
            HttpContext.Session.Remove("code");

            return Content(ReturnSuccess("注册成功"));
        }

        //忘记密码视图
        public IActionResult FindPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserFindPasswordAsync()
        {
            string email = Request.Form["email"];
            if (string.IsNullOrEmpty(email))
            {
                return Content(ReturnError("请输入注册时填写的邮箱"));
            }
            string account = Request.Form["account"];
            if (string.IsNullOrEmpty(account))
            {
                return Content(ReturnError("请输入登录账号"));
            }

            string vcode = Request.Form["vcode"];
            if (string.IsNullOrEmpty(vcode))
            {
                return Content(ReturnError("请输入登录验证码"));
            }
            //验证验证码
            var serverCode = HttpContext.Session.GetString("code");
            if (serverCode == null)
            {
                return Content(ReturnError("请输入登录验证码"));
            }
            int result = string.Compare(serverCode, vcode, true);
            if (result > 0)
            {
                return Content(ReturnError("验证码错误"));
            }

            //判断账户和邮箱是否存在
            if (!_userService.IsExistEmail(email))
            {
                return Content(ReturnError("邮箱不存在"));
            }

            if (!_userService.IsExistAccount(account))
            {
                return Content(ReturnError("帐号不存在"));
            }

            if(!_userService.CheckEmailAndAccount(email,account))
            {
                return Content(ReturnError("帐号或邮箱错误"));
            }

            //更新token和时间
            if(!_userService.UpdateEmailTokenAndExpire(email,account))
            {
                return Content(ReturnError("邮箱密钥更新失败"));
            }
            string scheme = Request.Scheme;
            string host = Request.Host.Host;
            int port = Request.Host.Port.Value;
            bool http = Request.IsHttps;
            //string html = "<a href='http'>找回密码链接</a>";

            await MailHelper.SendMailAsync(true, "忘记密码找回 - 极晖网络", email, account + "找回密码链接", "http://net.jihuiweb.com");
            //清除验证码
            HttpContext.Session.Remove("code");

            return Content(ReturnSuccess("邮件发送成功"));
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

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(ApplicationKeys.User_Cookie_Key);
            return Redirect(WebContext.LoginUrl);
        }
    }
}
