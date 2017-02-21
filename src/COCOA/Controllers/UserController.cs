using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using COCOA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace COCOA.Controllers
{
    /// <summary>
    /// Controller to handle user related actions like register, login, etc..
    /// </summary>
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController (UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register (string username, string password, string name)
        {
            // TODO: Implement Register & Login (endpoints + views).
            var user = new User { UserName = username, name = name };
            var result = await _userManager.CreateAsync(user, password);

            int i = 0;

            return Ok();
        }
    }
}
