using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class MaterialPDF
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Views { get; set; } = 0;
        public byte[] Data { get; set; }

        public Course Course { get; set; }
    }
}
