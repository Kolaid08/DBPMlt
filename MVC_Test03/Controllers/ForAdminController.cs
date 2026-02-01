using MVC_Test03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Test03.Controllers
{
    public class ForAdminController : Controller
    {

         private DBSportStoreEntities db = new DBSportStoreEntities();

    public ActionResult OrderStatistics()
    {
            // Lấy danh sách thống kê đơn hàng theo tháng
            var statistics = db.OrderProes
          .Where(o => o.DateOrder.HasValue)  // Đảm bảo có ngày đặt hàng
          .GroupBy(o => o.DateOrder.Value.Month)
          .Select(g => new
          {
              Month = g.Key,
              OrderCount = g.Count(),
              TotalRevenue = g.Sum(o => o.OrderDetails.Sum(od => (double)(od.Quantity * od.UnitPrice))), 
              TotalProductsSold = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity)) // Tính tổng số lượng sản phẩm
          })
          .AsEnumerable()  // Thực hiện truy vấn lên database
          .Select(g => new OrderStatisticsViewModel
          {
              Month = g.Month,
              OrderCount = g.OrderCount,
              TotalRevenue = (decimal)g.TotalRevenue,
              TotalProductsSold = (int)g.TotalProductsSold
          })
          .ToList();

            return View(statistics);

           
    }
        // GET: ForAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}