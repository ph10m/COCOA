using COCOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class CoursePageViewModel : CourseSubViewModel
    {
        public string courseName { get; set; }

        public string courseDescription { get; set; }

        public List<string> courseManagment { get; set; }

        public IEnumerable<BulletinViewModel> bulletins { get; set; }
        public IEnumerable<BulletinViewModel> stickyBulletins { get; set; }
    }
}
