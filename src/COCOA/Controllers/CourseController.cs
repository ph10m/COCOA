using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;
using COCOA.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace COCOA.Controllers
{
    public class CourseController : Controller
    {
        private readonly CocoaIdentityDbContext _context;
        private readonly UserManager<User> _userManager;

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
                //nextLecture = nextLect
            };

            return View(viewModel);
        }

        /// <summary>
        /// View for searching in course material. /materialsearch
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MaterialSearch()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var courses = await(from cE in _context.Enrollments
                                where cE.UserId == user.Id
                                select cE.Course).ToListAsync();

            return View("DocumentSearch", courses);
        }

        /// <summary>
        /// View for uploading documents /documentupload
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DocumentUpload()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var courses = await (from cA in _context.CourseAssignments
                                 where cA.UserId == user.Id
                                 select cA.Course).ToListAsync();

            return View(courses);
        }

        /// <summary>
        /// View for register course. /register
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// View for creating bulletin course. /createbulletin
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateBulletin()
        {
            return View("CreateBulletin");
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

            var courseAssignments = await (
                from cA in _context.CourseAssignments
                where cA.UserId == user.Id
                select cA).ToListAsync();

            // Any user with a course assigment can upload PDF material.
            if (courseAssignments.FirstOrDefault((cA) => { return (cA.CourseId == courseId); }) != null)
            {
                var materialPDF = new MaterialPDF
                {
                    AuthorId = user.Id,
                    CourseId = courseId,
                    Name = name,
                    Description = description,
                    Timestamp = DateTime.Now,
                    Data = data
                };

                _context.MaterialPDFs.Add(materialPDF);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Upload(string name, int courseId, string description)
        {
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                Request.Body.CopyTo(ms);
                bytes = ms.ToArray();
            }

            var courses = await (
                from c in _context.Courses
                where c.Id == courseId
                select c).ToListAsync();
 
            if (courses.Count == 0)
            {
                return StatusCode(404, "No matching course was found");
            }

            bool success = await SaveMaterialPDF(courses.First().Id, name, description, bytes);

            if (success)
                return Ok();
            else
                return StatusCode(400, "You are not eligible to upload to the given course");
        }

        // TODO: restrict action to teachers only.
        public async Task<IActionResult> NewCourse(string name, string description, string name1024)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var course = new Course
            {
                Name = name,
                Description = description,
                Name1024 = name1024,
                Timestamp = DateTime.Now
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var courseAssigment = new CourseAssignment
            {
                CourseId = course.Id,
                UserId = user.Id,
                CourseAssignmentRole = CourseAssignment.Role.Owner,
                Timestamp = DateTime.Now
            };

            _context.CourseAssignments.Add(courseAssigment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> NewBulletin (int courseId, string title, string content, string href, BulletinType bulletinType, bool stickey)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var courseAssignments = await (
                from cA in _context.CourseAssignments
                where cA.UserId == user.Id
                select cA).ToListAsync();

            // Any user with a course assigment can create a bulletin.
            if (courseAssignments.FirstOrDefault((cA) => { return (cA.CourseId == courseId); }) != null)
            {
                var courseBulletin = new CourseBulletin
                {
                    AuthorId = user.Id,
                    CourseId = courseId,
                    Title = title,
                    Content = content,
                    Href = href,
                    BulletinType = bulletinType,
                    Stickey = stickey,
                    Timestamp = DateTime.Now
                };

                _context.CourseBulletins.Add(courseBulletin);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return StatusCode(400, "User not assigned to course.");
        }

        public async Task<IActionResult> EnrollToCourse (int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var course = await (
                from c in _context.Courses
                where c.Id == id
                select c).SingleOrDefaultAsync();

            if (course != null)
            {
                var enrollment = new Enrollment
                {
                    CourseId = course.Id,
                    UserId = user.Id,
                    EnrollmentTimestamp = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return StatusCode(400, "Course not found.");
        }

        public async Task<IActionResult> AssignToCourse(int id, string userId, CourseAssignment.Role role)
        {
            var course = await (from c in _context.Courses
                                where c.Id == id
                                select c).SingleOrDefaultAsync();

            if (course == null)
            {
                return StatusCode(400, "Course not found!");
            }

            var userToAssign = await (from u in _context.Users
                                      where u.Id == userId
                                      select u).SingleOrDefaultAsync();

            if (userToAssign == null)
            {
                return StatusCode(400, "Users not found!");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userAssignment = await (from cA in _context.CourseAssignments
                                        where (cA.CourseId == course.Id && cA.UserId == user.Id)
                                        select cA).SingleOrDefaultAsync();

            if (userAssignment == null || userAssignment.CourseAssignmentRole == CourseAssignment.Role.Assistant)
            {
                return StatusCode(400, "Currently signed in user does not have the rights to assign a new user to this course!");
            }

            var newCourseAssignment = new CourseAssignment
            {
                CourseId = course.Id,
                UserId = userToAssign.Id,
                CourseAssignmentRole = role,
                Timestamp = DateTime.Now
            };

            _context.CourseAssignments.Add(newCourseAssignment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> DocumentSearch(int courseId, string searchString, int page = 0)
        {
            var result = await (from m in _context.MaterialPDFs
                                where (m.CourseId == courseId && m.Name.Contains(searchString))
                                select m.Meta).Skip(10 * page).Take(10).ToListAsync();

            /*string pdfPath = System.IO.Path.GetFullPath("..\\..\\example.pdf");
            using (PdfReader reader = new PdfReader(pdfPath))
            {
                searchString = searchString.ToLower();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string page = PdfTextExtractor.GetTextFromPage(reader, i).ToLower();
                    if (page.IndexOf(searchString) != -1)
                    {
                        string url = System.IO.Path.GetFullPath(pdfPath) + "#page=" + i;
                        return Ok(url);
                    }
                }
            }*/
            return Json(result);
        }
    }
}
