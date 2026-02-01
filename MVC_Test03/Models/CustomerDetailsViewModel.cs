using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; }
        public List<OrderHistoryViewModel> OrderHistory { get; set; }
    }
}