﻿using System;
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
        private readonly UserManager<IdentityUser> _usermanager;

        public AdminController(RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager)
        {
            _rolemanager = roleManager;
            _usermanager = userManager;
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
        [HttpGet]
        public async Task<IActionResult> EditRoles(string id) 
        {
            var role = await _rolemanager.FindByIdAsync(id);
            

            if(role == null)
            {
                ViewBag.ErrorMessage($"The Role ID : {id} was not found");
                return View("NotFound");
            }

            var model = new EditRoles
            {
                Id = role.Id,
                RoleName = role.Name,
                
            };
            foreach(var user in _usermanager.Users)
            {
                if(await _usermanager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(EditRoles model)
        {
            if(ModelState.IsValid)
            {
                var role = await _rolemanager.FindByIdAsync(model.Id);
                //Cannot find the specific role by id
                if(role == null)
                {
                    ViewBag.ErrorMessage($"The Role Id = {model.Id} could not be found");
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    var result = await _rolemanager.UpdateAsync(role);
                    if(result.Succeeded)
                    {
                        return View("EditRoles");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            
        }
        

        
    }
}