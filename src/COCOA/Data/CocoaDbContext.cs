﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using COCOA.Models;

namespace COCOA.Data
{
    /// <summary>
    /// Database context for code-first migration.
    /// </summary>
    public class CocoaDbContext : IdentityDbContext<User>
    {
        public CocoaDbContext(DbContextOptions<CocoaDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
