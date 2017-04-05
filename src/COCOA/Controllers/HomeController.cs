using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            var courses = await (
                from e in _context.Enrollments
                where e.UserId == user.Id
                select new Course()
                {
                    Name = e.Course.Name,
                    Name1024 = e.Course.Name1024,
                    Id = e.Course.Id,
                    Description = e.Course.Description
                }).ToListAsync();

            if (courses == null)
            {
                return StatusCode(400, "Couldn't fetch courses");
            }

            return View(courses);
        }
    }
}