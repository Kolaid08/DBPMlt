using MVC_Test03.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVC_Test03.Controllers
{
    public class HomeController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();
        public ActionResult Index()
        {
            var diningRoomProducts = db.Products
                 .Where(p => p.Category == "NTPA")
                 .OrderBy(x => Guid.NewGuid())
                 .Take(3)
                 .ToList();

            // Lấy 3 sản phẩm ngẫu nhiên cho Nội thất phòng khách
            var livingRoomProducts = db.Products
                .Where(p => p.Category == "NTPK")
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .ToList();

            // Lấy 3 sản phẩm ngẫu nhiên cho Nội thất phòng ngủ
            var bedroomProducts = db.Products
                .Where(p => p.Category == "NTPN")
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .ToList();

            // Tạo một ViewModel để nhóm các sản phẩm
            var viewModel = new HomeIndexViewModel
            {
                DiningRoomProducts = diningRoomProducts,
                LivingRoomProducts = livingRoomProducts,
                BedroomProducts = bedroomProducts
            };

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}