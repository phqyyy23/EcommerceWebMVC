using EcommerceWebMVC.Data;
using EcommerceWebMVC.IOrderState;
using EcommerceWebMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var hangHoas = db.HangHoas
                .Include(hh => hh.MaLoaiNavigation)  // Load thông tin loại hàng
                .Include(hh => hh.MaNccNavigation)   // Load thông tin nhà cung cấp
                .ToList();

            return View(hangHoas);
        }

        
        public ActionResult CreateProduct()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                hangHoa.SoLanXem = null; 
                db.HangHoas.Add(hangHoa);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hangHoa);
        }

        
        public ActionResult EditProduct(int id)
        {
            var hangHoa = db.HangHoas.Find(id);
            return View(hangHoa);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(int id, HangHoa hangHoa)
        {
            
            if (ModelState.IsValid)
            {
                db.Update(hangHoa);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hangHoa);
        }
        
        public ActionResult DeleteProduct(int id)
        {
            var hangHoa = db.HangHoas.Find(id);
            
            db.HangHoas.Remove(hangHoa);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction("Index");
        }
        public ActionResult ListCate()
        {
            var loai = db.Loais.ToList();
            return View(loai);
        }
        public ActionResult CreateCate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCate(Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Loais.Add(loai);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(loai);
        }

        
        public ActionResult EditCate(int id)
        {
            var  loai = db.Loais.Find(id);
            if (loai == null)
            {
                return NotFound();
            }
            return View(loai);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCate(int id, HangHoa loai)
        {
            if (ModelState.IsValid)
            {
                db.Update(loai);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(loai);
        }
        
        public ActionResult DeleteCate(int id)
        {
            var loai = db.HangHoas.Find(id);

            db.Remove(loai);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Danh mục đã được xóa thành công!";
            return RedirectToAction("Index");
        }

        //Quản lý đơn hàng
        public ActionResult Orders()
        {
            var orders = db.HoaDons
                .Include(o => o.MaTrangThaiNavigation)
                .Include(o => o.MaKhNavigation)
                .ToList();
            return View(orders);
        }

        public ActionResult NextStep(int orderId)
        {
            var order = db.HoaDons.Find(orderId);
            if (order != null)
            {
                var context = new OrderContext(order, db);
                if (order.MaTrangThai == 1) // 1 = Chờ xác nhận
                {
                    context.SetState(new ConfirmedState()); // Xác nhận đơn
                }
                else if (order.MaTrangThai == 2) // 2 = Đã xác nhận
                {
                    context.SetState(new ShippingState()); // Đang giao hàng
                }
                else if (order.MaTrangThai == 3) // 3 = Đang giao
                {
                    context.SetState(new DeliveredState()); // Hoàn thành
                }

            }
            return RedirectToAction(nameof(Orders));
        }   

        public ActionResult CancelOrder(int orderId)
        {
            var order = db.HoaDons.Find(orderId);
            if (order != null)
            {
                var context = new OrderContext(order, db);
                if (order.MaTrangThai == 1)
                {
                    context.SetState(new CancelledState()); // Luồng 2: Chờ xác nhận -> Hủy
                }
            }
            return RedirectToAction(nameof(Orders));
        }
        public ActionResult PayOrder(int orderId)
        {
            var order = db.HoaDons.Find(orderId);
            if (order != null)
            {
                var context = new OrderContext(order, db);
                context.SetState(new PaidState()); // Luồng 3: Thanh toán 
            }
            return RedirectToAction(nameof(Orders));
        }



    }
}
