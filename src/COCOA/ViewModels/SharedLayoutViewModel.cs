using COCOA.Data;
using COCOA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    /// <summary>
    /// A shared layout ViewModel to pass shared layout data to the header.
    /// </summary>
    public class SharedLayoutViewModel
    {
        public string userName { get; set; }

        public bool isTeacher { get; private set; }

        public List<CourseListItemViewModel> enrolledCourses { get; private set; }

        public List<CourseListItemViewModel> assignedCourses { get; private set; }

        public async Task<string> SetSharedDataAsync (CocoaIdentityDbContext dbContext, UserManager<User> userManager, User user)
        {
            enrolledCourses = new List<CourseListItemViewModel>();
            assignedCourses = new List<CourseListItemViewModel>();
            isTeacher = false;

            if (user != null)
            {
                userName = user.Name;

                // TODO: Use roles for this.
                isTeacher = user.Email.EndsWith("@ntnu.no");

                enrolledCourses = await (
                    from e in dbContext.Enrollments
                    where e.UserId == user.Id
                    select new CourseListItemViewModel
                    {
                        courseId = e.Course.Id,
                        courseName = e.Course.Name,
                        courseDescription = e.Course.Description
                    }).ToListAsync();

                assignedCourses = await (
                    from cA in dbContext.CourseAssignments
                    where cA.UserId == user.Id
                    select new CourseListItemViewModel
                    {
                        courseId = cA.Course.Id,
                        courseName = cA.Course.Name,
                        courseDescription = cA.Course.Description
                    }).ToListAsync();

                if (enrolledCourses == null)
                {
                    return "Couldn't fetch enrolled courses.";
                }

                if (assignedCourses == null)
                {
                    return "Couldn't fetch assigned courses.";
                }
            }

            return null;
        }
    }
}
