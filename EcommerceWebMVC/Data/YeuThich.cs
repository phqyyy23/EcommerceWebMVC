using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class YeuThich
{
    public int MaYt { get; set; }

    public int? MaHh { get; set; }

    public int? MaKh { get; set; }

    public string? MoTa { get; set; }

    public virtual HangHoa? MaHhNavigation { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }
}
