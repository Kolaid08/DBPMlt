using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class OrderStatisticsViewModel
    {
        public int Month { get; set; }  // Tháng
        public int OrderCount { get; set; }  // Số lượng đơn hàng
        public decimal TotalRevenue { get; set; }  // Tổng doanh thu
        public int TotalProductsSold { get; set; } //Tổng số sản phẩm đã bán
    }
}