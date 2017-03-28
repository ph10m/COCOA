using COCOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class CourseViewModel
    {
        public List<CourseBulletin> bulletins { get; set; }
        public List<CourseBulletin> stickyBulletins { get; set; }
    }
}
