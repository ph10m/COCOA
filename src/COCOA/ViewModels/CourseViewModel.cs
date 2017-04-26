using COCOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class CoursePageViewModel : SharedLayoutViewModel
    {
        public int courseId { get; set; }

        public string courseName { get; set; }

        public string courseDescription { get; set; }

        public string courseCoordinator { get; set; }

        public string courseInfolink { get; set; }


        public IEnumerable<BulletinViewModel> bulletins { get; set; }
        public IEnumerable<BulletinViewModel> stickyBulletins { get; set; }
    }
}
