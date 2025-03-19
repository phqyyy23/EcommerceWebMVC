using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public int? MaKh { get; set; }

    public DateTime? NgayDat { get; set; }

    public DateTime? NgayGiao { get; set; }

    public string? HoTen { get; set; }

    public string? DiaChi { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public int? MaVanChuyen { get; set; }

    public int? MaTrangThai { get; set; }

    public string? GhiChu { get; set; }

    public string? DienThoai { get; set; }

    public virtual ICollection<CthoaDon> CthoaDons { get; set; } = new List<CthoaDon>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual TrangThai? MaTrangThaiNavigation { get; set; }

    public virtual VanChuyen? MaVanChuyenNavigation { get; set; }
}
