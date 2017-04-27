using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Identity;
using COCOA.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;

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
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new CoursePageViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            var enrollment = await (from e in _context.Enrollments
                                    where e.UserId == user.Id && e.CourseId == id
                                    select e)
                                   .Include(x => x.Course)
                                   .SingleOrDefaultAsync();

            var assignment = await (from cA in _context.CourseAssignments
                                    where cA.UserId == user.Id && cA.CourseId == id
                                    select cA)
                                    .Include(x => x.Course)
                                    .SingleOrDefaultAsync();

            if (enrollment == null && assignment == null)
            {
                return StatusCode(400, "User not enrolled or assigned to course.");
            }

            var bulletins = await (from b in _context.CourseBulletins
                                   where b.CourseId == id
                                   select new BulletinViewModel
                                   {
                                       id = b.Id,
                                       authorName = b.Author.Name,
                                       title = b.Title,
                                       content = b.Content,
                                       href = b.Href,
                                       publishedDate = b.Timestamp.ToShortDateString() + "   " + b.Timestamp.ToShortTimeString(),
                                       publishedDateUnix = (long)b.Timestamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                                       bulletinType = b.BulletinType,
                                       stickey = b.Stickey
                                    }).OrderByDescending(x => x.publishedDateUnix).ToListAsync();

            //var coordinator = await (from cA in _context.CourseAssignments
            //                       where cA.CourseId == id
            //                       select (cA.CourseAssignmentRole.ToString() + ": " + cA.User.Name)).ToListAsync();
            var coordinator = await (from cA in _context.Courses
                                     where cA.Id == id
                                     select cA.Coordinator.ToString()).SingleOrDefaultAsync();
            var infolink = await (from cA in _context.Courses
                                     where cA.Id == id
                                     select cA.Infolink.ToString()).SingleOrDefaultAsync();



            var sticky = bulletins.Where(x =>
            {
                return x.stickey;
            });

            var normal = bulletins.Where(x =>
            {
                return !x.stickey;
            });

            model.courseName = enrollment?.Course.Name ?? assignment.Course.Name;
            model.courseId = enrollment?.Course.Id ?? assignment.Course.Id;
            model.assigned = (assignment != null);
            model.courseDescription = enrollment?.Course.Description ?? assignment.Course.Description;
            model.courseCoordinator = coordinator;
            model.courseInfolink = infolink;
            model.bulletins = normal;
            model.stickyBulletins = sticky;

            return View("Index", model);
        }

        /// <summary>
        /// View for searching in course material. /materialsearch
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MaterialSearch(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = await GetCourseSubViewModel(user, id);

            return View("DocumentSearch", model);
        }

        /// <summary>
        /// View for uploading documents /documentupload
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DocumentUpload(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = await GetCourseSubViewModel(user, id);

            return View("DocumentUpload", model);
        }

        /// <summary>
        /// View for register course. /register
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View("Register", model);
        }

        private async Task<CourseSubViewModel> GetCourseSubViewModel(User user, int id)
        {
            var model = new CourseSubViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            var enrollment = await (from e in _context.Enrollments
                                    where e.UserId == user.Id && e.CourseId == id
                                    select e)
                                   .Include(x => x.Course)
                                   .SingleOrDefaultAsync();

            var assignment = await (from cA in _context.CourseAssignments
                                    where cA.UserId == user.Id && cA.CourseId == id
                                    select cA).SingleOrDefaultAsync();

            var course = await (from c in _context.Courses
                                where c.Id == id
                                select c).SingleOrDefaultAsync();

            model.assigned = (assignment != null);
            model.courseId = id;
            model.courseName = course.Name;

            return model;
        }

        /// <summary>
        /// View for creating bulletin course. /createbulletin
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateBulletin(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = await GetCourseSubViewModel(user, id);

            return View("CreateBulletin", model);
        }

        /// <summary>
        /// View for enrollment to courses. /enrollment
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Enrollment()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View("Enrollment", model);
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
        [HttpPost]
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

            if (course == null)
            {
                return StatusCode(400, "Course not found.");
            }

            var existing = await (from e in _context.Enrollments
                                  where ((e.CourseId == course.Id) &&
                                  (e.UserId == user.Id))
                                  select e)
                                  .SingleOrDefaultAsync();

            if (existing != null)
            {
                return StatusCode(400, "Already enrolled to course.");
            }

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

        public async Task<IActionResult> DocumentSearch(int courseId, string searchString, int page = 0)
        {
            // the code that you want to measure comes here
            var result = await (from m in _context.MaterialPDFs
                                where (m.CourseId == courseId && (m.Name.Contains(searchString) || m.Description.Contains(searchString)))
                                select new { id =  m.Id, name = m.Name, description = m.Description} ).ToListAsync();

            result = result.OrderBy(mpmd => mpmd.name.Contains(searchString)?0:1)
                .Skip(10 * page).Take(10).ToList();

            
            return Json(result);
        }

        public async Task<IActionResult> CourseSearch(string searchString, int page = 0)
        {
            var result = await (from c in _context.Courses
                                where (c.Name.Contains(searchString))
                                select c).Skip(10 * page).Take(10).ToListAsync();
            return Json(result);
        }

        public async Task GetDocumentData(int documentId)
        {
            //TODO: Restrict to users of course

            var doc = await (from m in _context.MaterialPDFs
                             where (m.Id == documentId)
                             select m).SingleOrDefaultAsync();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition",
                "filename=\"" 
                + WebUtility.UrlEncode(doc.Name)
                + ".pdf\"");

            await Response.Body.WriteAsync(doc.Data, 0, doc.Data.Length);

            //Response.
            //Response.Flush();

            //Response.End();
        }

        public async Task<IActionResult> GetAssignedCourses()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            var courses = await (from c in _context.Courses
                                  join a in _context.CourseAssignments
                                  on c.Id equals a.CourseId
                                  join u in _context.Users
                                  on a.UserId equals u.Id
                                  where u.Id == user.Id
                                  select c)
                                  .ToArrayAsync();

            return Json(courses);
        }

        public async Task<IActionResult> GetEnrolledCourses()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            var courses = await (from c in _context.Courses
                                 join e in _context.Enrollments
                                 on c.Id equals e.CourseId
                                 join u in _context.Users
                                 on e.UserId equals u.Id
                                 where u.Id == user.Id
                                 select c)
                                  .ToArrayAsync();

            return Json(courses);
        }
    }
}
