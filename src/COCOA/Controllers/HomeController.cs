using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using COCOA.ViewModels;

namespace COCOA.Controllers
{
    //[RequireHttps]
    //[Authorize(Policy = "Authenticated")]
    public class HomeController : Controller
    {
        private readonly CocoaIdentityDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(CocoaIdentityDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userName = "";
            var model = new HomePageViewModel();
            if (user == null)
            {
                userName = "you do not seem to be logged in!";
            }
            else
            {
                userName = user.Name;
            }
            model.userName = userName;
            string resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View(model);
        }
    }
}