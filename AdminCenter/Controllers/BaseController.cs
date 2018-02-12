using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;
using SqlSugar;
using Utility;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminCenter.Controllers
{
    public class BaseController : Controller
    {
        public string ReturnMsg(ReturnEnum type, string msg, object data)
        {
            if (data == null)
            {
                return JsonConvert.SerializeObject(new { code = (int)type, msg = msg });
            }
            else
            {
                return JsonConvert.SerializeObject(new { code = (int)type, msg = msg, data = data });
            }
        }

        public string ReturnSuccess(string msg, object data = null)
        {
            return ReturnMsg(ReturnEnum.成功, msg, data);
        }

        public string ReturnError(string msg)
        {
            return ReturnMsg(ReturnEnum.失败, msg, null);
        }
    }
}
