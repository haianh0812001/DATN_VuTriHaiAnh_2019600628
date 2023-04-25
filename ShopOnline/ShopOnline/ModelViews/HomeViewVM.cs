using ShopOnline.Models;
using System.Collections.Generic;

namespace ShopOnline.ModelViews
{
    public class HomeViewVM
    {
        public List<TinDang>? TinTucs { get; set; }
        public List<ProductHomeVM>? Products { get; set; }
        public QuangCao? quangcao { get; set; }
        public List<Slider>? Sliders { get; set; }
    }
}
