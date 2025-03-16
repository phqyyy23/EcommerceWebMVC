using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebMVC.ViewModels
{
    public class RegisterVM
    {
        [Key]
        public int MaKH { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(30, ErrorMessage = "Tối đa 30 kí tự")]
        public string TenDn { get; set; } = null!;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!.%*?&]{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, một chữ in hoa và một ký tự đặc biệt.")]
        public string MatKhau { get; set; } = null!;

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string HoTen { get; set; } = null!;

        [Display(Name = "Giới tính")]
        public bool? GioiTinh { get; set; } = true;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        [CustomValidation(typeof(RegisterVM), nameof(ValidateNgaySinh))]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string DiaChi { get; set; } = null!;

        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "*")]
        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        [RegularExpression(@"^(0[2-9][0-9]{8,9})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string DienThoai { get; set; } = null!;


        public string? Hinh { get; set; }

        // Custom validation cho Ngày sinh (không được là ngày tương lai)
        public static ValidationResult? ValidateNgaySinh(DateTime? ngaySinh, ValidationContext context)
        {
            if (ngaySinh.HasValue && ngaySinh.Value > DateTime.Now)
            {
                return new ValidationResult("Ngày sinh không hợp lệ");
            }
            return ValidationResult.Success;
        }
    }
}
