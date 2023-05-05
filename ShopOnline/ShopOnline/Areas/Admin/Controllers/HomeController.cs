using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        
        private readonly dbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public HomeController(dbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            int currentMonth = DateTime.Now.Month; // Lấy tháng hiện tại
            DateTime fromDate = new DateTime(DateTime.Now.Year, currentMonth, 1); // Ngày bắt đầu của tháng
            DateTime toDate = new DateTime(DateTime.Now.Year, currentMonth, DateTime.DaysInMonth(DateTime.Now.Year, currentMonth)); // Ngày kết thúc của tháng

            ViewBag.TotalMoney = _context.Orders
                .AsNoTracking()
                .Where(x => x.Paid == true)
                .Sum(x => x.TotalMoney); 
            ViewBag.MoneyMonth = _context.Orders
                .AsNoTracking()
                .Where(x => x.OrderDate > fromDate && x.OrderDate < toDate && x.Paid == true)
                .Sum (x => x.TotalMoney);
            var lsOrders = _context.Orders
                .AsNoTracking()
                .Where(x => x.OrderDate > fromDate && x.OrderDate < toDate && x.Paid == true)
                .Take(6)
                .OrderByDescending(x => x.TotalMoney)
                .ToList();
            ViewBag.lsOrders = lsOrders;
            ViewBag.Donhang = _context.Orders.Count();
            ViewBag.Khachhang = _context.Customers.Count();
            ViewBag.Cholayhang = _context.Orders.Where(x => x.TransactStatusId == 1).Count();
            ViewBag.Choxacnhan = _context.Orders.Where(x => x.TransactStatusId == 2).Count();
            ViewBag.Danggiao = _context.Orders.Where(x => x.TransactStatusId == 3).Count();
            ViewBag.Dagiaothanhcong = _context.Orders.Where(x => x.TransactStatusId == 4).Count();
            ViewBag.Dahuy = _context.Orders.Where(x => x.TransactStatusId == 5).Count();
            ViewBag.Trahang = _context.Orders.Where(x => x.TransactStatusId == 6).Count();
            return View();
        }
    }
}
