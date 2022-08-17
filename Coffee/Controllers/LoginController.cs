using System;
using Coffee.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Coffee.Controllers
{
    public class LoginController : Controller
    {   
        
        // Tiêm phụ thuôc
        private readonly website_cafeMVCContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, website_cafeMVCContext context)
        {
            _logger = logger;
            _context = context;
        }      
        /*------------------Return view SignIn----------------------------*/
        public IActionResult SignIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        /*------------------Return view AcessDenied----------------------------*/

        public IActionResult AccessDenied()
        {
            return View();
        }
        /*------------------Return view SignUp----------------------------*/
        public IActionResult SignUp()
        {
            return View();
        }
       

    }
}
