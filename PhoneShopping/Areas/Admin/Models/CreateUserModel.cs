using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneShopping.Areas.Admin.Models
{
    public class CreateUserModel
    {
        [Key]
        public Guid Id { set; get; }

        public string Salt { set; get; }

        [Display(Name ="Username")]
        [Required(ErrorMessage = "Please input username")]
        public string UserName { set; get; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please input email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { set; get; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please input password")]
        public string Password { set; get; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please input confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { set; get; }

        public DateTime? RegisteredDate { get; set; }        
    }
}