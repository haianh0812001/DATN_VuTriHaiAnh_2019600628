using Microsoft.AspNetCore.Mvc;
using ShopOnline.Extension;
using ShopOnline.ModelViews;

namespace ShopOnline.Controllers.Components
{
    public class NumberCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}

