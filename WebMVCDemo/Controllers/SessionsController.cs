using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVCDemo.Data;

namespace WebMVCDemo.Controllers
{
    public class SessionsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SessionsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        
        
        //Functionality for login into the application
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username);
            if(user != null)
            {
                //sign the user in.
            }
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
        //Functionality for register a new user.
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };
            var result = await _userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {
                //sign user here
            }
            return RedirectToAction("Index", "Home");

        }
    }
}