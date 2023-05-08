using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Extension;
using ShopOnline.Helpper;
using ShopOnline.Models;
using ShopOnline.ModelViews;
using System.Security.Claims;

namespace ShopOnline.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AccountsController(dbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ThongBao()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.Orders
                        .Include(x => x.TransactStatus)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == khachhang.CustomerId)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }

            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(taikhoan.Email);
                    if (!isEmail) return View(taikhoan);
                    var check = _context.Customers.FirstOrDefault(x => x.Email == taikhoan.Email);
                    var check2 = _context.Accounts.FirstOrDefault(x => x.Email == taikhoan.Email);
                    if (check == null || check2 == null)
                    {
                        string salt = Utilities.GetRandomKey();
                        Customer khachhang = new Customer
                        {
                            FullName = taikhoan.FullName,
                            Phone = taikhoan.Phone.Trim().ToLower(),
                            Email = taikhoan.Email.Trim().ToLower(),
                            Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                            Active = true,
                            Salt = salt,
                            CreateDate = DateTime.Now
                        };
                        try
                        {
                            _context.Add(khachhang);
                            await _context.SaveChangesAsync();
                            //Lưu Session MaKh
                            HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                            var taikhoanID = HttpContext.Session.GetString("CustomerId");

                            //Identity
                            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);
                            _notyfService.Success("Đăng ký thành công");
                            return RedirectToAction("Dashboard", "Accounts");
                        }
                        catch
                        {
                            return RedirectToAction("DangkyTaiKhoan", "Accounts");
                        }
                    }
                    else
                    {
                        _notyfService.Success("Email đã được sử dụng!");
                        return RedirectToAction("DangkyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(ModelViews.LoginViewModel account, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == account.UserName );
                    var admin = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == account.UserName);
                    if (admin == null && khachhang == null)
                    {
                        _notyfService.Error("Thông tin đăng nhập chưa chính xác!");
                        return View(account);
                    }
                    if (khachhang == null)
                    {
                        string pass = (account.Password + admin.Salt.Trim()).ToMD5();
                        
                        if (admin.Password != pass)
                        {
                            _notyfService.Error("Thông tin đăng nhập chưa chính xác!");
                            return View(account);
                        }
                    //kiem tra xem account co bi disable hay khong

                        if (admin.Active == false)
                        {
                            return RedirectToAction("ThongBao", "Accounts");
                        }
                        HttpContext.Session.SetString("AccountID", admin.AccountId.ToString());
                        var taikhoanAD = HttpContext.Session.GetString("AccountID");
                        //Identity   
                        var claims2 = new List<Claim>
                        {
                            new Claim(type: ClaimTypes.Name, admin.FullName),
                            new Claim("AccountID", admin.AccountId.ToString()),
                        };
                        ClaimsIdentity claimsIdentity2 = new ClaimsIdentity(claims2, "login");
                        ClaimsPrincipal claimsPrincipal2 = new ClaimsPrincipal(claimsIdentity2);
                        await HttpContext.SignInAsync(claimsPrincipal2);
                        _notyfService.Success("Đăng nhập thành công");
                        return RedirectToAction("Home", "Admin");
                    }
                    else
                    {
                        string pass = (account.Password + khachhang.Salt.Trim()).ToMD5();
                        if (khachhang.Password != pass)
                        {
                            _notyfService.Error("Thông tin đăng nhập chưa chính xác!");
                            return View(account);
                        }
                    //kiem tra xem account co bi disable hay khong

                        if (khachhang.Active == false)
                        {
                            return RedirectToAction("ThongBao", "Accounts");
                        }
                        //Luu Session MaKh
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity   
                        var claims = new List<Claim>
                        {
                            new Claim(type: ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString()),
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng nhập thành công");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch
            {
                _notyfService.Error("Thông tin đăng nhập chưa chính xác!");
                return View(account);
            }
            return View(account);
        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword(ModelViews.ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
            }
            catch
            {
                _notyfService.Error("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Error("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "Accounts");
        }
        [HttpPost]
        public IActionResult UpdateProfile(ModelViews.UpdateProfileViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));

                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    {
                        
                        taikhoan.FullName = model.FullName;
                        taikhoan.Birthday = model.Birthday;
                        taikhoan.Address = model.Address;
                        taikhoan.Phone = model.Phone;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Thay đổi thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
            }
            catch
            {
                _notyfService.Error("Thông tin thay đổi không thành công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Error("Thông tin thay đổi không thành công");
            return RedirectToAction("Dashboard", "Accounts");
        }

    }
}
