﻿using ShopOnline.Models;

namespace ShopOnline.ModelViews
{
    public class CartItem
    {
        public Product? product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.Price.Value;
    }
}
