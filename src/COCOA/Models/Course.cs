using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Name1024 { get; set; }

        public ICollection<MaterialPDF> MaterialPDFs { get; set; }
        public User User { get; set; }
    }
}
