using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopOnline.ModelViews
{
    public class UpdateProfileViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        
        [MaxLength(255)]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string? FullName { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateTime? Birthday { get; set; }
        public string? Avatar { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string? Address { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string? Phone { get; set; }
    }
}
