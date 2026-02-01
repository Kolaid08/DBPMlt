using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Test03.CustomAuthorize
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRole = HttpContext.Current.Session["RoleUser"] as string;

            // Kiểm tra nếu UserRole không phải là "admin" hoặc "customer"
            if (string.IsNullOrEmpty(userRole) || userRole != "Admin" && userRole != "Customer")
            {
                // Chuyển hướng đến trang Login
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}