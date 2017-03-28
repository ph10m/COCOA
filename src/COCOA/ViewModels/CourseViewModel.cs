using COCOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class CourseViewModel
    {
        public IEnumerable<BulletinViewModel> bulletins { get; set; }
        public IEnumerable<BulletinViewModel> stickyBulletins { get; set; }
    }
}
