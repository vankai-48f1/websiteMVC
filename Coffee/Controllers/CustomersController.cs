using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Coffee.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly website_cafeMVCContext _context;

        public CustomersController(website_cafeMVCContext context)
        {
            _context = context;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
              return View();
        }


        /*-------------------Return file json-----------------*/
        [Route("jsonCustomer")]
        public IActionResult getCustomer()
        {
            var json_customer = _context.Customers.ToList();
            return new JsonResult(json_customer);
        }
        /*--------------------return view detail--------------------*/
        [Route("detail")]
        public async Task<IActionResult> Details(int? id)   
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
       
        private bool CustomerExists(int id)
        {
          return _context.Customers.Any(e => e.Id == id);
        }
    }
}
