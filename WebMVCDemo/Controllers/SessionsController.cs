using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMVCDemo.Controllers
{
    public class SessionsController : Controller
    {
        public IActionResult Login()
        {
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
    }
}