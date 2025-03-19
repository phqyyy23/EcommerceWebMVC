using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using EcommerceWebMVC.Data;
using EcommerceWebMVC.Helper;
using EcommerceWebMVC.Models;
using EcommerceWebMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceWebMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly EcommerceWebContext db;
        private readonly IMapper _mapper;

        public KhachHangController(EcommerceWebContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile? Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                    var existingUser = db.KhachHangs.FirstOrDefault(kh => kh.TenDn == model.TenDn);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("TenDn", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                        return View(model);
                    }

                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = null; // Hoặc khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                    khachHang.MatKhau = model.MatKhau;
                    khachHang.MaPq = 2;
                    khachHang.SoDiem = 0;
                    khachHang.Hinh = null;

                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi đăng ký: {ex.Message}");
                    Console.WriteLine($"Chi tiết lỗi: {ex.StackTrace}");

                    ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.TenDn == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (khachHang.MatKhau != model.Password)
                    {
                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                    }
                    else
                    {
                        if (khachHang.MaPq == 1)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim("AdminID", khachHang.MaKh.ToString()),
                                new Claim(ClaimTypes.Role, "Admin")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            return RedirectToAction("Index", "Admin");

                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim(MySetting.CLAIM_ID_KH, khachHang.MaKh.ToString()),
                                new Claim(ClaimTypes.Role, "User")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }

                }
            }
            return RedirectToAction("Index","Home");
        }
        


        [Authorize(Roles = "User")]
        public IActionResult Profile(int id)
        {
            // Kiểm tra nếu chưa đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            // Lấy ID khách hàng từ Claims (Nếu id == null, lấy từ session đăng nhập)
            int userId = int.Parse(User.FindFirst("CustomerID")?.Value ?? "0");

            // Tìm khách hàng trong DB
            var data = db.KhachHangs.SingleOrDefault(p => p.MaKh == userId);

            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy khách hàng mã {userId}";
                return Redirect("/404");
            }

            var result = new RegisterVM
            {
                MaKH = data.MaKh,
                TenDn = data.TenDn,
                HoTen = data.HoTen,
                MatKhau=data.MatKhau,
                Hinh = data.Hinh ?? "",
                NgaySinh = data.NgaySinh,
                GioiTinh = data.GioiTinh,
                DiaChi = data.DiaChi,
                DienThoai = data.DienThoai
            };

            return View(result);
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        

    }
}
