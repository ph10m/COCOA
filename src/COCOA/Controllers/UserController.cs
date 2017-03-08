using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using COCOA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        public UserController (UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            // Dependency injection
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// User managment page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View for register. /register
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// View for signin. /signin
        /// </summary>
        /// <returns></returns>
        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Register a new user in Cocoa.
        /// </summary>
        /// <param name="email">Domain names @stud.ntnu.no or @ntnu.no required</param>
        /// <param name="name">Full name for user</param>
        /// <param name="password">Password. Will be hashed by ASP.NET Identity</param>
        /// <returns>Returns Ok(200) on success. (400) on fail.</returns>
        [AllowAnonymous]
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

                    if (resultRole.Succeeded)
                    {
                        // Sign in
                        await _signInManager.SignInAsync(user, true);

                        return Ok();
                    }

                }
            }

            // Error
            return StatusCode(400, resultCreate?.Errors);
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
                return Ok();
            }

            // Error
            //return StatusCode(401);
            return StatusCode(401);
        }

        /// <summary>
        /// Signs out user.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> SignOut ()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
