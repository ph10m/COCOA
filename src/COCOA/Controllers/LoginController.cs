using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace COCOA.Controllers
{
    //[RequireHttps]
    //[Authorize(Policy = "Authenticated")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
