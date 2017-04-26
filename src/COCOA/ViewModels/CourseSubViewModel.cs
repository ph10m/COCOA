using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class CourseSubViewModel : SharedLayoutViewModel
    {
        public int courseId { get; set; }

        public string courseName { get; set; }

        public bool assigned { get; set; }
    }
}
