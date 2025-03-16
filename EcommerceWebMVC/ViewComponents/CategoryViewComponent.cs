using EcommerceWebMVC.Data;
using EcommerceWebMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebMVC.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly EcommerceWebContext db;
        public CategoryViewComponent(EcommerceWebContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new CategoryVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);

            return View(data); //default.cshtml (đường dẫn mặc định)
            //return View("Default",data);
        }
    }
}

