using System;
using System.Collections.Generic;

namespace EcommerceWebMVC.Data;

public partial class Ncc
{
    public int MaNcc { get; set; }

    public string TenCongTy { get; set; } = null!;

    public string? Email { get; set; }

    public string? DienThoai { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
