using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVCDemo.Models;

namespace WebMVCDemo.Controllers
{
    /*
     * Controller and Razor pages work with data that comes from HTTP request.
     * For example perhaps the route data may have some data for us to work with
     * or form fields have values that we need. Trying to retrieve this and convert
     * them to the proper types would be tedious and error prone. In ASP.NET, model
     * biding attempts to automate this process.
     * Example
     * Public ActionResult GetById(int id, bool dogsOnly)
     * Model binding sees that the first parameter is and id. It looks through the
     * available sources in the HTTP request and finds a valid value. Once it does,
     * it will then attempt to convert it to the correct type if possible. It will then
     * do the same thing for dogsOnly
     *
     */
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
            //Reshaprer tool. The brown/red? underline means that the view exists for this action exists.
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(Roles model)
        {
            if (!ModelState.IsValid) return View("Error");

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
                        //return View("Error");
                        return RedirectToAction("ListRoles");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View("Error");
        }
        //This methods does two things. First it gets a list of users in the user database. Then based on the
        //roleId it recieved checks whether the user is in the given role or not. If they are, then
        //we flag them as such. This is to help us determined in the view and to display to the user
        //whether the user is already in the role when they're are adding users to the role.
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _rolemanager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.ErrorMessage($"Role with Id {roleId} cannot be found");
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();

            foreach (var user in _usermanager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await _usermanager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            if (!ModelState.IsValid) return View("Error");

            var role = await _rolemanager.FindByIdAsync(roleId);

            IdentityResult result = null;

            if (role == null)
            {
                ViewBag.ErrorMessage($"The User with Id {roleId} could not be found");
                return View("Error");
            }

            foreach (var userRole in model)
            {
                var user = await _usermanager.FindByIdAsync(userRole.UserId);
                if (userRole.IsSelected && !(await _usermanager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _usermanager.AddToRoleAsync(user,role.Name);
                }
                else if (!userRole.IsSelected && await _usermanager.IsInRoleAsync(user, role.Name))
                {
                    result = await _usermanager.RemoveFromRoleAsync(user, role.Name);
                }

                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage($"The user could not be remove from role");
                    return View("NotFound");
                }
            }

            return RedirectToAction("EditRoles", new {Id = roleId});
        }

        

        
    }
}