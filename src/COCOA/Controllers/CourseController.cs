using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;

namespace COCOA.Controllers
{
    public class CourseController : Controller
    {
        public CocoaIdentityDbContext _context;
        public UserManager<User> _userManager;

        public CourseController (CocoaIdentityDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> NewCourse (string name, string description, string name1024)
        {
            var course = new Course();
            course.Name = name;
            course.Description = description;
            course.Name1024 = name1024;
            course.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> EnrollToCourse (int id)
        {
            var enrollment = new Enrollment();
            enrollment.CourseId = id;
            enrollment.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            enrollment.EnrollmentTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            _context.Enrollments.Add(enrollment);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
