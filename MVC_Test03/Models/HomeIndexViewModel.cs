using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class HomeIndexViewModel
    {
        public List<Product> DiningRoomProducts { get; set; }
        public List<Product> LivingRoomProducts { get; set; }
        public List<Product> BedroomProducts { get; set; }
    }
}