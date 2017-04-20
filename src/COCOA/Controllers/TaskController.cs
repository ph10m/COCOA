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


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace COCOA.Controllers
{
    public class TaskController : Controller
    {
        private readonly CocoaIdentityDbContext _context;
        private readonly UserManager<User> _userManager;

        public TaskController(CocoaIdentityDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            string resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View(model);
        }
    }
}
