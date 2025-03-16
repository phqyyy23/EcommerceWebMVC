using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class VanChuyen
{
    public int MaVanChuyen { get; set; }

    public string PhuongThuc { get; set; } = null!;

    public decimal PhiVanChuyen { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
