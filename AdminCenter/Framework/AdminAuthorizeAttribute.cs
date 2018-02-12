using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminCenter.Application;

namespace AdminCenter.Framework
{
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            if (string.IsNullOrEmpty(WebContext.AdminName))
            {
                if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    filterContext.Result = new JsonResult("未登录");
                }
                else
                {
                    filterContext.Result = new RedirectResult(WebContext.LoginUrl);
                }
                return;
            }
        }
    }
}
