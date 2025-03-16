using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebMVC.Data;

public partial class KhachHang
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaKh { get; set; }

    public string TenDn { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? Hinh { get; set; }

    public int? MaPq { get; set; }

    public string? RandomKey { get; set; }

    public int? SoDiem { get; set; }

    public string? MatKhau { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual PhanQuyen? MaPqNavigation { get; set; }

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}
