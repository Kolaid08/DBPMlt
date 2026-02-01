using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class OrderHistoryViewModel
    {
        public DateTime DateOrder { get; set; }      
        public string AddressDeliverry { get; set; }  
        public List<OrderDetailViewModel> OrderDetails { get; set; } 
    }
}