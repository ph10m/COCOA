using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public long EnrollmentTimestamp { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
