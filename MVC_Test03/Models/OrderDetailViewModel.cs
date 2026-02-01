using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class OrderDetailViewModel
    {
        public int IDProduct { get; set; }  // ID sản phẩm
        public int Quantity { get; set; }    // Số lượng
        public float UnitPrice { get; set; }
    }
}