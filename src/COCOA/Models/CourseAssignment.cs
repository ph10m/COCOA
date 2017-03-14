using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class CourseAssignment
    {
        public enum Role
        {
            Owner,
            Instructor,
            Assistant
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public Role CourseAssignmentRole { get; set; }        
        
        public DateTime Timestamp { get; set; }
    }
}