using EcommerceWebMVC.Data;
using EcommerceWebMVC.ViewModels;
using EcommerceWebMVC.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.CodeAnalysis.Scripting;
using System;


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
        [HttpPost]
        public IActionResult UpdateQuantity(int id, string action)
        {
            var giohang = Cart ?? new List<CartItem>();

            var cartItem = giohang.FirstOrDefault(c => c.IdProd == id);
            if (cartItem != null)
            {
                if (action == "increase")
                {
                    cartItem.SoLuong++;
                }
                else if (action == "decrease")
                {
                    if (cartItem.SoLuong > 1)
                    {
                        cartItem.SoLuong--;
                    }
                    else
                    {
                        giohang.Remove(cartItem);
                    }
                }
                

                // Lưu lại session
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);

                decimal cartTotal = (decimal)giohang.Sum(p => p.ThanhTien);

                return Json(new
                {
                    success = true,
                    newQuantity = cartItem.SoLuong,
                    newTotal = cartItem.SoLuong * cartItem.DonGia,
                    cartTotal =  cartTotal
                });
            }
            
            return Json(new { success = false });
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

        [Authorize]
        [HttpGet]
        public IActionResult ThanhToan()
        {
            
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }
        [Authorize]
        [HttpPost]
        public IActionResult ThanhToan(ThanhToanVM model)
        {
            if (ModelState.IsValid)
            {
                var customerID = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_ID_KH).Value);
                var khachHang = new KhachHang();
                if(model.GiongKhachHang)
                {
                     khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerID);
                }
                var hoadon = new HoaDon
                {
                    MaKh = customerID,
                    HoTen = model.HoTen ?? khachHang.HoTen,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    DienThoai = model.DienThoai ?? khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    PhuongThucThanhToan = "COD",
                    MaVanChuyen = 1,
                    MaTrangThai = 1,
                    GhiChu = model.GhiChu
                };
                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthds = new List<CthoaDon>();
                    foreach (var item in Cart)
                    {
                        cthds.Add(new CthoaDon
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = (decimal)item.DonGia,
                            MaHh = item.IdProd,
                            GiamGia = 0
                        });
                    }
                    db.AddRange(cthds);
                    db.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {

                    db.Database.RollbackTransaction();
                }
                
               
            }
            return View(Cart);
        } 
            
    }
}
