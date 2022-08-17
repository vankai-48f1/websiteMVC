using Coffee.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coffee.Controllers
{
    public class HomeController : Controller
    {
        // Tiêm phụ thuôc
        private readonly website_cafeMVCContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, website_cafeMVCContext context)
        {
            _logger = logger;
            _context = context;
        }
        /*----------------------------------------  View of Homecontrollers -------------------------------*/
        public IActionResult Index()
        {
            return View();
        }     
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogDetail()
        {
            return View();
        }
        public IActionResult Setting()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
      
    }
}
