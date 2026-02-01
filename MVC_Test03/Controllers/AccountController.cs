using MVC_Test03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Test03.Controllers
{
    public class AccountController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();

        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Thêm vào bảng Customer
                var customer = new Customer
                {
                    NameCus = model.FullName,
                    PhoneCus = model.PhoneNumber,
                    EmailCus = model.Email
                };
                db.Customers.Add(customer);
                db.SaveChanges();

                // Thêm vào bảng AdminUser
                var adminUser = new AdminUser
                {
                    ID = customer.IDCus,  // Sử dụng IDCus làm ID cho AdminUser
                    NameUser = model.Username,
                    PasswordUser = model.UserPassword,
                    RoleUser = "Customer"  // Gán giá trị mặc định là "Customer"
                };
                db.AdminUsers.Add(adminUser);

                db.SaveChanges();
                return RedirectToAction("Index", "Home");  // Điều hướng về trang chủ sau khi đăng ký thành công
            }

            return View(model);
        }
    
        

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            // Kiểm tra người dùng từ cơ sở dữ liệu
            var user = db.AdminUsers.FirstOrDefault(u => u.NameUser == username && u.PasswordUser == password);
            if (user != null)
            {
                // Nếu user hợp lệ, thiết lập session
                Session["UserRole"] = user.RoleUser; // Gán role cho session
                Session["UserID"] = user.ID;
                FormsAuthentication.SetAuthCookie(username, false); // Đăng nhập

                // Kiểm tra và chuyển hướng đến returnUrl nếu hợp lệ
                if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
                }
            }

            // Nếu không hợp lệ, trả về thông báo lỗi
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(); // Trả về view Login với lỗi
        }

        //Đổi mật khẩu
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Session["UserID"] as int?;
                var user = db.AdminUsers.Find(userId);
                ViewBag.UserPassword = user.PasswordUser.Trim();
                var matkhaugoc = user.PasswordUser.Trim();
                if (matkhaugoc != model.CurrentPassword.Trim())
                {
                    ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                    return View(model); // Trả về view với thông báo lỗi
                }
                    user.PasswordUser = model.NewPassword;
                    db.SaveChanges();

                // Thêm thông báo thành công
                ViewBag.Message = "Đổi mật khẩu thành công!";

                // Chuyển hướng đến trang Details sau khi đổi mật khẩu thành công
                return RedirectToAction("Details", "Customers");
            }

            // Nếu có lỗi, trả về lại form
            return View(model);
        }


        [HttpGet]
        public JsonResult GetUserRole()
        {
            var userRole = Session["UserRole"] as string;
            return Json(new { role = userRole }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

