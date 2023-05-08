using System;
using System.Collections.Generic;

namespace ShopOnline.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            Products = new HashSet<Product>();
        }

        public int Nccid { get; set; }
        public string? Nccname { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
