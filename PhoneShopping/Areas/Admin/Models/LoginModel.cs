using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneShopping.Areas.Admin.Models
{
    public class LoginModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please input your email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { set; get; }
        [Required(ErrorMessage = "Please input password")]
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}