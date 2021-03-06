﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using COCOA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using COCOA.ViewModels;
using COCOA.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace COCOA.Controllers
{
    /// <summary>
    /// Controller to handle user related actions like register, login, etc..
    /// </summary>
    [RequireHttps]
    public class UserController : Controller
    {
        // Controller dependencies
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CocoaIdentityDbContext _context;

        private readonly TimeSpan tokenExpiration = new TimeSpan(30, 0, 0, 0);
        private readonly SymmetricSecurityKey signingKey = StartupTests.signingKey;
        private readonly SigningCredentials signingCredentials;

        public UserController (UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, CocoaIdentityDbContext context)
        {
            // Dependency injection
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

            signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// User coordinator page.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View(model);
        }

        /// <summary>
        /// View for register. /register
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View(model);
        }

        /// <summary>
        /// View for signin. /signin
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SignIn()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new SharedLayoutViewModel();
            var resultShared = await model.SetSharedDataAsync(_context, _userManager, user);

            if (resultShared != null)
            {
                return StatusCode(400, resultShared);
            }

            return View(model);
        }

        /// <summary>
        /// Register a new user in Cocoa.
        /// </summary>
        /// <param name="email">Domain names @stud.ntnu.no or @ntnu.no required</param>
        /// <param name="name">Full name for user</param>
        /// <param name="password">Password. Will be hashed by ASP.NET Identity</param>
        /// <returns>Returns Ok(200) on success. (400) on fail.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(string email, string name, string password)
        {
            var user = new User { UserName = email, Email = email, Name = name };

            IdentityResult resultCreate = null;

            if (email.EndsWith("@stud.ntnu.no") || email.EndsWith("@ntnu.no"))
            {
                // Student or teacher?
                var role = (email.EndsWith("@stud.ntnu.no") ? "Student" : "Teacher");

                resultCreate = await _userManager.CreateAsync(user, password);
                if (resultCreate.Succeeded)
                {
                    // Add role
                    var resultRole = await _userManager.AddToRoleAsync(user, role);

                    // Check if Teacher is a coordinator
                    if (role == "Teacher")
                    {
                        var result = await (from x in _context.Courses
                                            where x.Coordinator.Contains(email)
                                            select x).ToListAsync();

                        foreach (var course in result)
                        {
                            var newCourseAssignment = new CourseAssignment
                            {
                                CourseId = course.Id,
                                UserId = user.Id,
                                CourseAssignmentRole = CourseAssignment.Role.Owner,
                                Timestamp = DateTime.Now
                            };

                            _context.CourseAssignments.Add(newCourseAssignment);
                            await _context.SaveChangesAsync();
                        }

                    }

                    if (resultRole.Succeeded)
                    {
                        // Sign in
                        await _signInManager.SignInAsync(user, true);

                        return Ok();
                    }
                }
            }

            // Error
            return StatusCode(400, resultCreate?.Errors.First().Description ?? "Email address not under supported domain.");
        }

        /// <summary>
        /// Login to Cocoa.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="persistent">Remember session?</param>
        /// <returns>Returns Ok(200) on success. (401) on fail (wrong email/password)</returns>
        [AllowAnonymous]
        public async Task<IActionResult> SignInUser (string email, string password, bool persistent)
        {
            // Sign in
            var result = await _signInManager.PasswordSignInAsync(email, password, persistent, false);

            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                return new RedirectToActionResult("index", "home", null);
            }

            // Error
            //return StatusCode(401);
            return StatusCode(401);
        }

        /// <summary>
        /// Signs in user and returns a JWT token to be used in HTTP-request headers for authorization.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>JWT token</returns>
        [HttpPost]
        public async Task<IActionResult> SignInUserToken(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

            if (!result.Succeeded)
            {
                return StatusCode(400, "Invalid username or password.");
            }

            var id = (await _userManager.FindByEmailAsync(email)).Id;

            // Specifically add the jti (random nonce), iat (issued timestamp), and UniqueName (userId) claims.
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(tokenExpiration),
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Json(encodedJwt);
        }

        /// <summary>
        /// Signs out user.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SignOut ()
        {
            await _signInManager.SignOutAsync();

            return new RedirectToActionResult("signin", "user", null);
        }

        //[Authorize]
        public async Task<IActionResult> Name ()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var x = Request;

            return Json(user?.Name ?? "xx");
        }
    }
}
