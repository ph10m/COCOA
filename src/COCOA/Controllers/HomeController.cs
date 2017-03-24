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
            var courses = await (
                from c in _context.Courses
                select new Course()
                {
                    Name = c.Name,
                    Name1024 = c.Name1024,
                    Id = c.Id,
                    Description = c.Description
                }).ToListAsync();
            if (courses == null) return StatusCode(400, "Couldn't fetch courses");
            foreach (var courseInfo in courses)
            {
                // TODO: do something with each course
                Console.WriteLine(courseInfo.Id + ": " + courseInfo.Description);
            }
            return View(courses);
        }
    }
}
