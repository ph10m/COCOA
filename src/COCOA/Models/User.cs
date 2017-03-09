using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    /// <summary>
    /// User model
    /// </summary>
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public long RegisterTimestamp { get; set; }

        /// <summary>
        /// Courses a user is enrolled to.
        /// </summary>
        public ICollection<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Courses a user is assigned to as an Owner, Instructor or Assistant.
        /// </summary>
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
