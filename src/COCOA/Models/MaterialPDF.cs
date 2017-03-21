using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class MaterialPDF
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DefaultValue(0)]
        public int Views { get; set; }

        public DateTime Timestamp { get; set; }

        public byte[] Data { get; set; }

        public MaterialPDFMetadata Meta {
            get
            {
                MaterialPDFMetadata mpmd = new MaterialPDFMetadata();
                mpmd.description = Description;
                mpmd.id = Id;
                mpmd.name = Name;
                return mpmd;
            }

        }

        public class MaterialPDFMetadata
        {
            public int id { get; set; }

            public string name { get; set; }

            public string description { get; set; }
        }
        
    }
}
