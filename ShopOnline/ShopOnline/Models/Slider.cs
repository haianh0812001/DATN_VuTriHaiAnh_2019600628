using System;
using System.Collections.Generic;

namespace ShopOnline.Models
{
    public partial class Slider
    {
        public int SliderId { get; set; }
        public string? BgImage { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Alias { get; set; }
        public string? Contents { get; set; }
        public string? SliderName { get; set; }
    }
}
