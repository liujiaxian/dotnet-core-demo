using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminCenter.Controllers
{
    public class TokenController : Controller
    {
        [Route("/token")]
        [HttpPost]
        public IActionResult Create(string username, string password)
        {
            //if (IsValidUserAndPasswordCombination(username, password))
            //    return new ObjectResult(GenerateToken(username));
            //return BadRequest();
            return null;
        }
    }
}
