﻿using BusinessLogic.DB;
using Domain;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Web.Controllers
{
    [IPBanFilterAttribute]
    public class LoginController : Controller
    {
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string ipAddress2 = Request.ServerVariables["REMOTE_ADDR"];


            StorageEntities db = new StorageEntities();
            bool IsValidUser = db.User.Any(user => user.User_Login.ToLower() ==
             model.UserLogin && user.User_Password == model.Password);
            if (IsValidUser)
            {
                // get the users email
                User user = db.User.Where(tempik => tempik.User_Login == model.UserLogin).FirstOrDefault();
                // concatenate the user Data
                string userData = string.Format("{0}|{1}|{2}|{3}", model.UserLogin, model.Password, user.User_Email, user.User_Picture);
                // create a ticket that expires after N period of time
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserLogin, DateTime.Now, DateTime.Now.AddHours(1), false, userData);
                // setting authorization 
                FormsAuthentication.SetAuthCookie(model.UserLogin, false);
                // encrypting the ticket
                string encTicket = FormsAuthentication.Encrypt(ticket);
                // creating http cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                // adding the cookie to the collection
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
            }
            //ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }

        [HttpGet]
        public ViewResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserModel ulog)
        {
            StorageEntities db = new StorageEntities();
            User tempUser = db.User.Where(model => model.User_Login == ulog.Login || model.User_Login == ulog.Email).FirstOrDefault();
            if (tempUser == null)
            {
                // creating user 
                User user = new User();
                user.User_Login = ulog.Login;
                user.User_Password = ulog.Password;
                user.User_Email = ulog.Email;

                if (ulog.ProfilePicture != null && ulog.ProfilePicture.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ulog.ProfilePicture.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/images/pfp"), fileName);
                    ulog.ProfilePicture.SaveAs(filePath);
                    user.User_Picture = "/Content/images/pfp/" + fileName;
                    // Additional processing or saving logic here
                }
                else
                {
                    user.User_Picture = "~/Content/images/Your-place.png";
                }

                // saving user
                db.User.Add(user);

                // assigning default role to the user
                UserRole role = new UserRole();
                role.User_ID = user.User_ID;
                role.Role_ID = 2;
                db.UserRole.Add(role);

                // save it
                db.SaveChanges();
                return RedirectToAction("Login", "Login", ulog);
            }
            return View("login", ulog);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}