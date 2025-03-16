using EcommerceWebMVC.Data;
using EcommerceWebMVC.ViewModels;
using EcommerceWebMVC.Helper;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceWebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly EcommerceWebContext db;
        public CartController(EcommerceWebContext context)
        {
            db = context;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();




        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProd == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Not Found Product{id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    IdProd = hangHoa.MaHh,
                    TenProd = hangHoa.TenHh,
                    DonGia = (double)hangHoa.DonGia,
                    Hinh = hangHoa.Hinh ?? "",
                    SoLuong = quantity,
                };
                gioHang.Add(item);

            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProd == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            }
            return RedirectToAction("Index");
        }

    }
}
