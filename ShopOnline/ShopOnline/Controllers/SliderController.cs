using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopOnline.Models;

namespace ShopOnline.Controllers
{
    public class SliderController : Controller
    {
        private readonly dbMarketsContext _context;
        public SliderController(dbMarketsContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        [Route("blogs.html", Name = ("Blog"))]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsSlider = _context.Sliders
                .AsNoTracking()
                .OrderBy(x => x.SliderId);
            PagedList<Slider> models = new PagedList<Slider>(lsSlider, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/slider/{id}.html", Name = "SliderChiTiet")]
        public IActionResult Details(int id)
        {
            var slider = _context.Sliders.AsNoTracking().SingleOrDefault(x => x.SliderId == id);
            if (slider == null)
            {
                return RedirectToAction("Index");
            }
            return View(slider);
        }
    }
}
