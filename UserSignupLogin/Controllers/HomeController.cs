using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserSignupLogin.Models;

namespace UserSignupLogin.Controllers
{
    public class HomeController : Controller
    {
        DBuserSignupLoginEntities db = new DBuserSignupLoginEntities();
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                return View(db.TBLUserInfoes.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(TBLUserInfo tBLUserInfo)
        {
            try
            {
                if (db.TBLUserInfoes.Any(x => x.UserNameUs == tBLUserInfo.UserNameUs))
                {
                    ViewBag.Notification = "This account already exists";
                    return View();
                }
                else
                {
                    db.TBLUserInfoes.Add(tBLUserInfo);
                    db.SaveChanges();

                    Session["IdUsSS"] = tBLUserInfo.IdUs.ToString();
                    Session["UserNameSS"] = tBLUserInfo.UserNameUs.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(); ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.Where(x => x.UserNameUs.Equals(tBLUserInfo.UserNameUs) && x.PasswordUs.Equals(tBLUserInfo.PasswordUs)).FirstOrDefault();
            if(checkLogin != null)
            {
                Session["IdUsSS"] = tBLUserInfo.IdUs.ToString();
                Session["UserNameSS"] = tBLUserInfo.UserNameUs.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Incorrect Username or password";
            }
            return View();
        }
    }
}