using BotDetect.Web.Mvc;
using Models.DAO;
using Models.EF;
using PhoneShopping.Areas.Admin.Models;
using PhoneShopping.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //If user is trying to login over 3 times
            if(TempData.ContainsKey("LoginFormDisplayCaptcha"))
            {                
                MvcCaptcha mvcCaptcha = new MvcCaptcha("CheckCaptcha");
                if (mvcCaptcha.Validate(user.CaptchaCode))
                {                    
                    MvcCaptcha.ResetCaptcha("CheckCaptcha");
                }
                else
                {
                    ModelState.AddModelError("CaptchaCode", "Wrong Captcha!");
                }
            }

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var getUser = dao.getUserByEmail(user.Email);

                //If the email does not exist, show error messages
                if(getUser == null)
                {
                    ModelState.AddModelError("Email", "Email account does not exist in database.");
                } 
                else
                {
                    string securityStamp = getUser.SecurityStamp;
                    string password = getUser.Password;
                    string comparePassword = Helper.EncodePassword(user.Password, securityStamp);
                    if(getUser.LockoutEnd == null || getUser.AccessFailedCount == null || getUser.LockoutEnabled == null)
                    {
                        //If password is not matched, reset and count the attempted login
                        if (password.Equals(comparePassword))
                        {
                            var userSession = new UserLogin();
                            userSession.UserId = getUser.Id;
                            userSession.UserName = getUser.UserName;
                            userSession.Email = getUser.Email;
                            Session.Add(CommonConstants.USER_SESSION, userSession);
                            dao.resetCountAttemptedLogin(getUser.Id, 1, Helper.nextDay(DateTimeOffset.UtcNow), false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Password does not exist in database.");
                            dao.resetCountAttemptedLogin(getUser.Id, 1, Helper.nextDay(DateTimeOffset.UtcNow), false);
                        }
                        
                    } else
                    {
                        DateTime logoutEnd = ((DateTimeOffset)getUser.LockoutEnd).ToUniversalTime().Date;
                        int acessFailedCount = (int)getUser.AccessFailedCount;
                        bool lockoutEnabled = (bool)getUser.LockoutEnabled;
                        if (lockoutEnabled) //If account is blocked
                        {
                            //Check block is expired or not
                            DateTime currentDateTime = DateTimeOffset.UtcNow.ToUniversalTime().Date;
                            if (!Helper.SameDate(currentDateTime, logoutEnd)) //If block is expired, then check password
                            {
                                //If password is not matched, reset and count the attempted login
                                if (!password.Equals(comparePassword))
                                {
                                    ModelState.AddModelError("Password", "Password does not exist in database.");
                                }
                                else
                                {
                                    var userSession = new UserLogin();
                                    userSession.UserId = getUser.Id;
                                    userSession.UserName = getUser.UserName;
                                    userSession.Email = getUser.Email;
                                    Session.Add(CommonConstants.USER_SESSION, userSession);
                                    TempData.Remove("LoginFormDisplayCaptcha");
                                    TempData.Remove("LoginFailedErrorMessage");
                                    return RedirectToAction("Index", "Home");
                                }
                                dao.resetCountAttemptedLogin(getUser.Id, 1, Helper.nextDay(DateTimeOffset.UtcNow), false);
                            }
                            else
                            {
                                TempData["LoginFailedErrorMessage"] = "Your account is blocked";
                            }
                        }
                        else
                        {
                            //If password is not matched, reset and count the attempted login
                            if (!password.Equals(comparePassword))
                            {
                                int attemptLoginCaptcha = int.Parse(ConfigurationManager.AppSettings["AttemptLoginCaptcha"].ToString());
                                int attemptLoginLock = int.Parse(ConfigurationManager.AppSettings["AttemptLoginLock"].ToString());
                                if (acessFailedCount > attemptLoginCaptcha && acessFailedCount <= attemptLoginLock) //If number of login attempt exceeds 3, show captcha
                                {
                                    TempData["LoginFormDisplayCaptcha"] = "true";
                                    dao.countUserAttempt(getUser.Id, acessFailedCount + 1);
                                }
                                else if (acessFailedCount > attemptLoginLock) //If number of login attempt exceeds 8, disable account a day
                                {
                                    TempData["LoginFormDisplayCaptcha"] = "true";
                                    TempData["LoginFailedErrorMessage"] = "Your account is blocked";
                                    dao.disableAccount(getUser.Id, acessFailedCount + 1, true);
                                }
                                else
                                {
                                    dao.countUserAttempt(getUser.Id, acessFailedCount + 1);
                                }
                            }
                            else
                            {
                                var userSession = new UserLogin();
                                userSession.UserId = getUser.Id;
                                userSession.UserName = getUser.UserName;
                                userSession.Email = getUser.Email;
                                TempData.Remove("LoginFormDisplayCaptcha");
                                TempData.Remove("LoginFailedErrorMessage");
                                Session.Add(CommonConstants.USER_SESSION, userSession);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }                    
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
                entity.Id = Guid.NewGuid();
                entity.UserName = user.UserName;
                entity.Email = user.Email;                
                entity.SecurityStamp = Guid.NewGuid().ToString();
                entity.Password = Helper.EncodePassword(user.Password, entity.SecurityStamp);
                entity.RegisteredDate = DateTime.UtcNow;
                entity.Status = true;
                Guid id = dao.createUser(entity);
                if(id != null)
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