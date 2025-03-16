using EcommerceWebMVC.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly EcommerceWebContext db;
        public AdminController(EcommerceWebContext context)
        {
            db = context;
        }

        // GET: AdminController1
        public ActionResult Index()
        {
            var hangHoas = db.HangHoas
                .Include(hh => hh.MaLoaiNavigation)  // Load thông tin loại hàng
                .Include(hh => hh.MaNccNavigation)   // Load thông tin nhà cung cấp
                .ToList();

            return View(hangHoas);
        }

        // GET: AdminController1/Create
        public ActionResult CreateProduct()
        {
            return View();
        }

        // POST: AdminController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                db.HangHoas.Add(hangHoa);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hangHoa);
        }

        // GET: AdminController1/Edit/5
        public ActionResult EditProduct(int id)
        {
            var hangHoa = db.HangHoas.Find(id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            return View(hangHoa);
        }

        // POST: AdminController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(int id, HangHoa hangHoa)
        {
            if (id != hangHoa.MaHh)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                db.Update(hangHoa);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hangHoa);
        }
        // POST: AdminController1/Delete/5
        public ActionResult DeleteProduct(int id)
        {
            var hangHoa = db.HangHoas.Find(id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            db.HangHoas.Remove(hangHoa);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction("Index");
        }




    }
}
