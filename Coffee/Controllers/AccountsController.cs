using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace Coffee.Controllers
{

    [Route("accounts")]
    public class AccountsController : Controller
    {
        /*-------------------------------- Dependency injection-------------------------------*/
        private readonly website_cafeMVCContext _context;
        // Sử dụng môi trường máy chủ IWebHostEnvironment
        private readonly IWebHostEnvironment _hostEnvironment;
        public AccountsController(website_cafeMVCContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }


        /*--------------------------------Return View Index----------------------------------*/
        [Authorize(Policy = "Administrator")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var website_cafeMVCContext = _context.Accounts.Include(a => a.Customer).Include(a => a.Media);
            return View(await website_cafeMVCContext.ToListAsync());
        }


        /*--------------------------------------- Return View Details---------------------------*/
        [Authorize(Policy = "Administrator")]
        [Route("detail")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Media)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        /*---------------------------Handler Function Save --------------------------*/
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("save")]
        public async Task<IActionResult> save(int id, [Bind("FileUload")] Account acc )
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Id == id);
            var media = _context.Media.FirstOrDefault(x => x.Id == account.MediaId);
            string fileName = null;
            string extension = null;
            if (account == null) return NotFound();
            /* 
              - Lưu hình ảnh vào đường dẫn PATH: wwwroot/image
              - Phương thức GetFileNameWithoutExtension(string path) lấy tên đường dẫn hình ảnh mà không có phần mở rộng phía sau dấu chấm.
              - Phương thức GetExtension lấy phần mở rộng phía sau.
              - Gán đường dẫn và lưu lại trong database chèn theo một chuỗi thời gian để không trùng lặp.
              - Lấy  đường dẫn tuyệt đối đến thư mục chứa các tệp hình ảnh.
            */
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image");
            if (acc.FileUload == null) return View(nameof(Index));
            fileName = Path.GetFileNameWithoutExtension(acc.FileUload.FileName);    
            extension = Path.GetExtension(acc.FileUload.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                acc.FileUload.CopyTo(stream);   
            }
            if (media == null)
            {
                Medium m = new Medium();
                m.FileName = fileName;
                m.TimeCreate = DateTime.Now;
                m.Path = path;
                account.Media = m;
                _context.Add(m);
                await _context.SaveChangesAsync();
            }
            else
            {
                media.FileName = fileName;
                media.Path = path;
                media.TimeCreate = DateTime.Now;
                _context.Update(media);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index","accounts");
        }

        /* --------------------------handler Function Direct Detail Customer----------------*/
        [Authorize(Policy = "Administrator")]
        [Route("getCustomer")]
        public async Task<IActionResult> Details(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Media)
                .FirstOrDefaultAsync(x => x.CustomerId == id);
            return View(account);
        }



        /*------------------------------------Return View Create----------------------------------*/
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName");
            ViewData["MediaId"] = new SelectList(_context.Media, "Id", "FileName");
            return View();
        }
        /*
            Return json a list account 
         */
        [Route("jsonAccount")]
        public async Task<IActionResult> json_lstAccount()
        {
            var ajaxAccount = await _context.Accounts.ToListAsync();
            Debug.WriteLine(ajaxAccount);
            return new JsonResult(ajaxAccount);

        }



        /*-----------------------Handler function create account ----------------------*/
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Role")] Account account)
        {
            if (!ModelState.IsValid)
            {
                TempData["notification"] = "Không thành công";
                return RedirectToAction(nameof(Create));
            }
            else if (UserNameExists(account.Username))
            {
                TempData["userExists"] = "Tên đăng nhập đã tồn tại!";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                if (account.Password == Request.Form["ConfirmPassword"])
                {
                    account.Password = HashKey(account.Password);
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["notification"] = "Xác nhận mật khẩu không đúng";
                    return RedirectToAction(nameof(Create));
                }
            }
        }
        /*-----------------------Check Unique username-----------------------------------------*/
        public bool UserNameExists(string name)
        {
            return _context.Accounts.Any(x => x.Username == name);
        }
        public bool PhoneExists(string phone)
        {
            return _context.Customers.Any(x => x.Phone == phone);
        }
        public bool EmailExists(string email)
        {
            return _context.Customers.Any(x => x.Email == email);
        }

        /*-----------------------Handler function create account customer----------------------*/
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create_customer")]
        public async Task<IActionResult> createCustomer([Bind("Id,Username,Password")] Account account)
        {

            if (ModelState.IsValid)
            {
                /* if (UserNameExists(account.Username))
                 {

                 }*/
                var customer = new Customer
                {
                    FullName = Request.Form["Customer.FullName"],
                    Gender = Request.Form["Customer.Gender"],
                    Birth = Convert.ToDateTime(Request.Form["Customer.Birth.ToString()"]),
                    Email = Request.Form["Customer.Email"],
                    Phone = Request.Form["Customer.Phone"],
                    Address = Request.Form["Customer.Address"],
                };
                _context.Add(customer);
                account.Customer = customer;
                account.Password = HashKey(account.Password);
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("SignUp", "Login");

        }

        /*--------------------------Handler function reset password user---------------------------*/
        [Authorize(Policy = "Administrator")]
        [Route("reset")]
        public async Task<IActionResult> reset_pass(int id)
        {
            if (!ModelState.IsValid)
            {
                TempData["notification"] = "Không thành công";
                return View();
            }
            var e_account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            var e_customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == e_account.CustomerId);
            if (e_customer == null)
            {
                if (e_account == null) return NotFound();
                e_account.Password = HashKey("xincamon");
                _context.Accounts.Update(e_account);
                await _context.SaveChangesAsync();
                TempData["notification"] = "Không thành công";
                return View(nameof(Index));
            }
            else
            {
                e_account.Password = HashKey("xincamon");
                _context.Customers.Update(e_customer);
                _context.Accounts.Update(e_account);
                await _context.SaveChangesAsync();
                TempData["notification"] = "Không thành công";
                return View(nameof(Index));
            }
        }

        /*-----------------------------Handler function HashPassword-------------------------------------*/
        private string HashKey(string pass)
        {
            /* Sử dụng System.Security.Cryptography.
             * Tạo một thuật toán bảo mật SHA-256. mã hóa 1 chiều không thể đảo ngược và duy nhất.
            */
            var sha = SHA256.Create();
            // Lấy mật khẩu người dùng từ hệ thống.
            var inputPass = Encoding.Default.GetBytes(pass);
            // Tính toán và băm.
            var hash = sha.ComputeHash(inputPass);
            return Convert.ToBase64String(hash);
        }
        /*-----------------------------Tạo Quyền để đăng nhập--------------------------------------*/
        private async void createAuthorize(Account a)
        {
            // Kiểm tra quyền để tạo quyền truy cập
            var Claims = new List<Claim>();
            if (a.Role == "Administrator")
            {
                Claims.Add(new Claim(ClaimTypes.Name, a.Role));
                Claims.Add(new Claim("Admin", "true"));
            }
            else if (a.Role == "Staff")
            {
                Claims.Add(new Claim(ClaimTypes.Name, a.Role));
                Claims.Add(new Claim("Staff", "true"));
            }
            else
            {
                Claims.Add(new Claim(ClaimTypes.Name, a.Role));
                Claims.Add(new Claim("Customer", "true"));
            }
            var Identity = new ClaimsIdentity(Claims, "CookieAuth");
            var Principal = new ClaimsPrincipal(Identity);
            await HttpContext.SignInAsync("CookieAuth", Principal);
        }

        /*-----------------------------Handler function SignIn-------------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(Account obj)
        {
            // Check Validate model
            if (ModelState.IsValid)
            {
                // Xác thực
                // Băm mật khẩu của người dùng nhập vào.
                var a = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == obj.Username);
                if (a == null)
                {
                    TempData["Error"] = "Tài khoản chưa được đăng ký!";
                    return RedirectToAction("SignIn", "Login");
                }
                else if (!check_active(a))
                {
                    TempData["Error"] = "Tài khoản đang tạm khóa!";
                    return RedirectToAction("SignIn", "Login");
                }
                else
                {
                    if (a.Password.Equals(HashKey(obj.Password)))
                    {
                        createAuthorize(a);
                        TempData["content"] = "Xin chào! Đăng nhập thành công";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "Mật khẩu không chính xác!";
                        return RedirectToAction("SignIn", "Login");
                    }


                }
            }
            TempData["Error"] = "Mật khẩu không chính xác!";
            return RedirectToAction("SignIn", "Login");
        }
        /*-----------------------Handler Function SignOut-------------------------*/
        [Route("signout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("SignIn", "Login");
        }
        /*-----------------------Check Active--------------------------------------*/
        private bool check_active(Account a)
        {
            if (a.Status == true) return true;
            return false;
        }

        /*-----------------------Handler Fuction Block Accounts--------------------*/
        [HttpPost("switch_status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmActive(int Id)
        {
            var active_account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == Id);
            if (active_account == null) return View(nameof(Index));
            if (active_account.Status == true)
            {
                active_account.Status = false;
            }
            else
            {
                active_account.Status = true;
            }
            _context.Update(active_account);
            await _context.SaveChangesAsync();
            return RedirectToAction("index","accounts");
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }


    }
}
