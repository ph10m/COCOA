using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    public class CourseBulletin
    {
        public int Id { get; set; }

        /// <summary>
        /// What CourseId this bulletin has been posted to.
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// What Course this bulletin has been posted to.
        /// </summary>
        public Course Course { get; set; }

        /// <summary>
        /// AuthorId of this bulletin.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Author of this bulletin.
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Bulletin title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Bulletin body content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///  Link to external/internal content.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Type of the bulletin. Will be displayed differently in frontend depending on type.
        /// </summary>
        public BulletinType BulletinType { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Should the bulletin stick at the top of the board ?
        /// </summary>
        public bool Stickey { get; set; }
    }

    public enum BulletinType
    {
        Normal = 0,
        Info = 1,
        Urgent = 2
    }
}
