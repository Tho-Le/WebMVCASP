using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVCDemo.Models;

namespace WebMVCDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            _rolemanager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        public async Task<IActionResult> CreateRole(Roles model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _rolemanager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _rolemanager.Roles;
            return View(roles);
        } 
    }
}