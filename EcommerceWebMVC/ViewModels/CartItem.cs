    namespace EcommerceWebMVC.ViewModels
{
    public class CartItem
    {
        public int IdProd { get; set; }
        public string TenProd { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * DonGia;
        public int Id { get; set; }
        public int CustomerId { get; set; }
    }
}
