using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class HangHoa
{
    public int MaHh { get; set; }

    public string TenHh { get; set; } = null!;

    public string? Hinh { get; set; }

    public int? MaLoai { get; set; }

    public decimal DonGia { get; set; }

    public decimal? GiamGia { get; set; }

    public int? SoLanXem { get; set; }

    public string? MoTa { get; set; }

    public int? MaNcc { get; set; }

    public virtual ICollection<CthoaDon> CthoaDons { get; set; } = new List<CthoaDon>();

    public virtual Loai? MaLoaiNavigation { get; set; }

    public virtual Ncc? MaNccNavigation { get; set; }

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}
