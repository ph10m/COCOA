using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCOA.Models
{
    /// <summary>
    /// User model
    /// </summary>
    public class User : IdentityUser
    {
        public string name { get; set; }

        public string name1024 { get; set; }
    }
}
