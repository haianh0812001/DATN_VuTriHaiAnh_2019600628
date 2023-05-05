using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopOnline.Areas.Admin.Controllerst
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly dbMarketsContext _context;

        public SearchController(dbMarketsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.Products.AsNoTracking()
                    .Include(a => a.Cat)
                    .OrderByDescending(x => x.ProductName)
                    .ToList();
            }
            else
            {
                ls = _context.Products.AsNoTracking()
                    .Include(a => a.Cat)
                    .Where(x => x.ProductName.Contains(keyword))
                    .OrderByDescending(x => x.ProductName)
                    .ToList();
            }
            return PartialView("ListProductsSearchPartial", ls);
        }
    }
}
