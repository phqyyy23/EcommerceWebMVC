using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class CthoaDon
{
    public int MaCt { get; set; }

    public int? MaHd { get; set; }

    public int? MaHh { get; set; }

    public decimal DonGia { get; set; }

    public int SoLuong { get; set; }

    public decimal? GiamGia { get; set; }

    public virtual HoaDon? MaHdNavigation { get; set; }

    public virtual HangHoa? MaHhNavigation { get; set; }
}
