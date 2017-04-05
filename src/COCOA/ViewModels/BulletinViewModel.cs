using COCOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.ViewModels
{
    public class BulletinViewModel
    {
        public int id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string authorName { get; set; }

        public string href { get; set; }

        public BulletinType bulletinType { get; set; }

        public string publishedDate { get; set; }

        public long publishedDateUnix { get; set; }

        public bool stickey { get; set; }
    }
}
