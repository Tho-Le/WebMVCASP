using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebMVCDemo.Controllers
{
    public class SessionsController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}