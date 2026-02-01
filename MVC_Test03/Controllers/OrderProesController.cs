using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Test03.Models;
using PagedList;

namespace MVC_Test03.Controllers
{
    public class OrderProesController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();

        // GET: OrderProes
            [ActionName("IndexWithoutPagination")]
        public ActionResult Index()
        {
            var orderProes = db.OrderProes.Include(o => o.Customer);
            return View(orderProes.ToList());
        }

        // GET: OrderProes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin đơn hàng và chi tiết đơn hàng
            var orderPro = db.OrderProes
                .Include(o => o.OrderDetails)  // Bao gồm các chi tiết đơn hàng
                .FirstOrDefault(o => o.ID == id);

            if (orderPro == null)
            {
                return HttpNotFound();
            }

            return View(orderPro);
        }

        public ActionResult MyOrders(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // Lọc tất cả các đơn hàng của khách hàng theo IDCus
            var orders = db.OrderProes
                .Where(o => o.IDCus == id) // Lọc theo IDCus của khách hàng
                .Include(o => o.OrderDetails)  // Bao gồm chi tiết đơn hàng
                .ToList();

            if (orders == null || orders.Count == 0)
            {
                ViewBag.ErrorMessage = "Bạn chưa có đơn hàng nào .";
                
            }

            return View(orders); // Trả về view với danh sách đơn hàng
        }

        // POST: OrderProes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOrder,IDCus,AddressDeliverry")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.OrderProes.Add(orderPro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // GET: OrderProes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // POST: OrderProes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOrder,IDCus,AddressDeliverry")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderPro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // GET: OrderProes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        // POST: OrderProes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //OrderPro orderPro = db.OrderProes.Find(id);
            //if (orderPro != null)
            //{
            //    db.OrderProes.Remove(orderPro);
            //    db.SaveChanges();
            //    TempData["Message"] = "Đơn hàng đã được xóa thành công!";
            //}
            //return RedirectToAction("Index");
            OrderPro orderPro = db.OrderProes.Include("OrderDetails").FirstOrDefault(o => o.ID == id);
            if (orderPro != null)
            {
                // Xóa các chi tiết đơn hàng liên quan
                foreach (var detail in orderPro.OrderDetails.ToList())
                {
                    db.OrderDetails.Remove(detail);
                }

                // Xóa đơn hàng
                db.OrderProes.Remove(orderPro);
                db.SaveChanges();
                TempData["Message"] = "Đơn hàng đã được xóa thành công!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 10; // Số mục hiển thị trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là 1 nếu chưa có trang nào được chọn

            // Lấy danh sách OrderPro sắp xếp theo ngày đặt hàng
            var orders = db.OrderProes.OrderBy(o => o.DateOrder).ToPagedList(pageNumber, pageSize);

            return View(orders);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
