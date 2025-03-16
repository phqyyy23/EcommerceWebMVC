using System.Drawing.Printing;
using System.Linq;
using EcommerceWebMVC.Data;
using EcommerceWebMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceWebContext db;
        public ProductController(EcommerceWebContext context)
        {
            db = context;
        }
        //hiển thị danh sách sản phẩm
        public IActionResult Index(int? loai, int page = 1, int pageSize = 12)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);

            }
            // Tính tổng số sản phẩm
            var totalProducts = hangHoas.Count();

            // Lấy danh sách sản phẩm cho trang hiện tại
            var result = hangHoas.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(p => new ProductVM
                                  {
                                      IdProd = p.MaHh,
                                      TenProd = p.TenHh,
                                      DonGia = (double)p.DonGia,
                                      Hinh = p.Hinh ?? "",
                                      TenLoai = p.MaLoaiNavigation.TenLoai,
                                      MoTa = p.MoTa
                                  }).ToList();

            // Tính tổng số trang
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Gửi dữ liệu đến view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(result);
        }
        //tìm kiếm
        public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new ProductVM
            {
                IdProd = p.MaHh,
                TenProd = p.TenHh,
                DonGia = (double)p.DonGia,
                Hinh = p.Hinh ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai,
                MoTa = p.MoTa
            });

            return View(result);
        }
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm mã {id}";
                return Redirect("/404");
            }
            var result = new DetailProductVM
            {
                IdProd = data.MaHh,
                TenProd = data.TenHh,
                DonGia = (double)data.DonGia,
                Hinh = data.Hinh ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                DiemDanhGia = 5, //lam sau
                SoLuongTon = 10,
            };
            return View(result);
        }

    }
}
