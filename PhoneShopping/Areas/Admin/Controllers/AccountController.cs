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
                var dao = new UserDao();

                var getUser = dao.getUserByEmail(user.Email);

                string securityStamp = getUser.SecurityStamp;
                string password = getUser.Password;
                string comparePassword = Helper.EncodePassword(user.Password, securityStamp);

                if (password.Equals(comparePassword))
                {                    
                    var userSession = new UserLogin();
                    userSession.UserId = getUser.Id;
                    userSession.UserName = getUser.UserName;
                    userSession.Email = getUser.Email;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
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
            var dao = new UserDao();
            
            var getUserByEmail = dao.getUserByEmail(user.Email);            
            if (getUserByEmail != null)
            {
                ModelState.AddModelError("", "Email account exists in database.");
                ViewBag.AccountEmailCreateExistErrorMessage = "Email exists in database.";
            }

            var getUserByUsername = dao.getUserByEmail(user.UserName);
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
                long id = dao.CreateUser(entity);
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