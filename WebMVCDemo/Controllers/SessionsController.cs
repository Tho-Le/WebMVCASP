using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVCDemo.Data;

namespace WebMVCDemo.Controllers
{
    public class SessionsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityUser> _roleManager;

        public SessionsController(UserManager<IdentityUser> userManager
            ,SignInManager<IdentityUser> signInManager, ILoggerFactory logger)
            
        {
            //_roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger.CreateLogger("WebMVCDemo.Sessions.Controllers");
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        //Functionality for login into the application
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user != null)
            {
                //sign the user in.
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if(signInResult.Succeeded)
                {
                    _logger.LogInformation("User has signed in successfully");
                    return RedirectToAction("Index", "Home");
                }
            }

            _logger.LogInformation("User has unsuccessfully attempted to log in");

            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Authenticate()
        {
            return RedirectToAction("Fail");
        }
        public IActionResult Fail()
        {
            return View();
            
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //Functionality for register a new user.
        public async Task<IActionResult> Register(string username, string password)
        {

            var isValid = Validator.Validator.ValidatePassword(password);
            if(!isValid)
            {
                return RedirectToAction("password");
            }
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };
            var result = await _userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {
                //sign user here
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (username.Equals("administrator"))
                {
                    await _roleManager.CreateAsync(user);
                }
                await _userManager.AddToRoleAsync(user, "user");

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Fail");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("user has logged out successfull");
            return RedirectToAction("Index", "Home");
        }
        
    }
}