using ShopOnline.Models;

namespace ShopOnline.ModelViews
{
    public class XemDonHang
    {
        public Order? DonHang { get; set; }
        public List<OrderDetail>? ChiTietDonHang { get; set; }
    }
}
