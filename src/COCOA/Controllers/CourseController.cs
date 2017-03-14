using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;
using COCOA.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        
        public async Task<IActionResult> Index(int id)
        {
            var name1024 = await (
                from c in _context.Courses
                where c.Id == id
                select c.Name1024).SingleOrDefaultAsync();
            if (name1024 == null)
            {
                return StatusCode(400, "Course not found!");
            }
            var nextLect = new WebScraper(name1024).getNextLecture();

            var viewModel = new CourseViewModel
            {
                nextLecture = nextLect
            };

            return View();
        }

        /// <summary>
        /// Async call to save material PDF to database. User needs to be assigned to a course(Owner, Instructor or Assistant) to add a file for it.
        /// </summary>
        /// <param name="courseId">Course to add this MaterialPDF to</param>
        /// <param name="name">Name of file.</param>
        /// <param name="description">Description of file.</param>
        /// <param name="data">PDF binary data</param>
        /// <returns>Returns true if successfully saved, false if not.</returns>
        private async Task<bool> SaveMaterialPDF (int courseId, string name, string description, byte[] data)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Any user with a course assigment can upload PDF material.
            if (user.CourseAssignments.FirstOrDefault((cA) => { return (cA.CourseId == courseId); }) != null)
            {
                var materialPDF = new MaterialPDF();
                materialPDF.UserId = user.Id;
                materialPDF.CourseId = courseId;
                materialPDF.Name = name;
                materialPDF.Description = description;
                materialPDF.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                materialPDF.Data = data;

                // Add file to database
                _context.MaterialPDFs.Add(materialPDF);

                // Synchronize changes
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<IActionResult> NewCourse (string name, string description, string name1024)
        {
            var course = new Course();
            course.Name = name;
            course.Description = description;
            course.Name1024 = name1024;
            
            _context.Courses.Add(course);

            // Save new course to get id to use in CourseAssignment
            await _context.SaveChangesAsync();

            var courseAssigment = new CourseAssignment();
            courseAssigment.CourseId = course.Id;
            courseAssigment.UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            courseAssigment.CourseAssignmentRole = CourseAssignment.Role.Owner;

            _context.CourseAssignments.Add(courseAssigment);

            // Synchronize changes
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
