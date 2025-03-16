 using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EcommerceWebMVC.ViewModels
{
    public class ProductVM
    {
        public int IdProd { get; set; }
        public string TenProd { get; set; }
        public string Hinh { get; set; }
        public double DonGia {  get; set; } 
        public string TenLoai { get; set; }
        public string MoTa { get; set; }
    }
}
