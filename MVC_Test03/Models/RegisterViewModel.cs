using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Test03.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username không được bỏ trống.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password không được bỏ trống.")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Họ và tên không được bỏ trống.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

    }
}