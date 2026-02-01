using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class User
    {
        public int ID { get; set; }
        public string NameUser { get; set; }
        public string RoleUser { get; set; }
        public string PasswordUser { get; set; }
    }
}