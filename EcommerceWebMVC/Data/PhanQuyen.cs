using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class PhanQuyen
{
    public int MaPq { get; set; }

    public string ChucVu { get; set; } = null!;

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
