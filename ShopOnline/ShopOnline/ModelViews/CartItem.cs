using ShopOnline.Models;

namespace ShopOnline.ModelViews
{
    public class CartItem
    {
        public Product? product { get; set; }
        public int amount { get; set; }
        public double Discout => product.Discount.Value;
        public double DiscountMN => (100 - product.Discount.Value) / 100 * product.Price.Value;
        public double TotalMoney => amount * product.Price.Value;

    }
}
