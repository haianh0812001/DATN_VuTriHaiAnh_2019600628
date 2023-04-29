using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopOnline.Models
{
    public partial class Account
    {
        public Account()
        {
            TinDangs = new HashSet<TinDang>();
        }

        public int AccountId { get; set; }
        public string? Phone { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = ("Vui lòng nhập Email"))]
        [Display(Name = "Địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        public string? Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public bool Active { get; set; }
        public string? FullName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<TinDang> TinDangs { get; set; }
    }
}
