using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace AdminCenter.Application
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
        public static T GetService<T>() where T : class
        {
            return Instance.GetService<T>();
        }

    }
    public static class WebContext
    {
        public static string AdminName
        {
            get
            {
                //获取cookie
                var hasCookie = ServiceLocator.GetService<IHttpContextAccessor>()
                    .HttpContext
                    .Request.Cookies
                    .TryGetValue(ApplicationKeys.User_Cookie_Key, out string encryptValue);
                if (!hasCookie || string.IsNullOrEmpty(encryptValue))
                    return null;
                var adminName = ServiceLocator.GetService<IUserService>().LoginDecrypt(encryptValue, ApplicationKeys.User_Cookie_Encryption_Key);
                return adminName;
            }
        }
        public const string LoginUrl = "/account/login";
    }
}
