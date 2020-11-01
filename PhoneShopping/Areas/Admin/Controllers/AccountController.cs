using Models.DAO;
using Models.EF;
using PhoneShopping.Areas.Admin.Models;
using PhoneShopping.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneShopping.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var model = new UserDao();

                var getUser = model.getUserByEmail(user.Email);

                string securityStamp = getUser.SecurityStamp;
                string password = getUser.Password;
                string comparePassword = Helper.EncodePassword(user.Password, securityStamp);

                if (password.Equals(comparePassword))
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ViewBag.LoginFailedErrorMessage = "Username or password is not correct.";
                }
            }
            return View(user);
        }

        public ActionResult Create()
        {                   
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateUserModel user)
        {
            var model = new UserDao();

            if (user.IsAgree == false)
            {
                //check Agree terms and policy
                ModelState.AddModelError("", "Please agree the terms and policy.");
                ViewBag.AccountCreateErrorMessage = "Please agree the terms and policy.";
            }

            
            var getUserByEmail = model.getUserByEmail(user.Email);            
            if (getUserByEmail != null)
            {
                ModelState.AddModelError("", "Email account exists in database.");
                ViewBag.AccountEmailCreateExistErrorMessage = "Email exists in database.";
            }

            var getUserByUsername = model.getUserByEmail(user.UserName);
            if (getUserByUsername != null)
            {
                ModelState.AddModelError("", "Username exists in database.");
                ViewBag.AccountUsernameCreateExistErrorMessage = "Username exists in database.";
            }

            if (ModelState.IsValid)
            {
                var entity = new User();                
                entity.UserName = user.UserName;
                entity.Email = user.Email;                
                entity.SecurityStamp = Guid.NewGuid().ToString();
                entity.Password = Helper.EncodePassword(user.Password, entity.SecurityStamp);
                entity.RegisteredDate = DateTime.UtcNow;
                entity.Status = true;
                long id = model.CreateUser(entity);
                if(id > 0)
                {
                   return RedirectToAction("Login", "Account");
                } else
                {
                    ViewBag.CreateANewAccountFailedErrorMessage = "Create a new account failed.";                    
                }
            }
            return View(user);
        }

        public ActionResult Forgetpassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}