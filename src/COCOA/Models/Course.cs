using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Name1024 { get; set; }

        public DateTime Timestamp { get; set; }

        public string Coordinator { get; set; }

        public string Infolink { get; set; }
    }
}
