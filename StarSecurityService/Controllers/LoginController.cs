using StarSecurityService.Common;
using StarSecurityService.Dao;
using StarSecurityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StarSecurityService.Controllers
{
    public class LoginController : Controller
    {
        StarSecurityDataDataContext data = new StarSecurityDataDataContext();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(Account model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.username, model.password);
                if (result == 1)
                {
                    var user = dao.GetById(model.username);
                    var userSession = new UserLogin();
                    userSession.UserName = user.username;
                    userSession.UserID = user.id;
                    userSession.Role = user.role;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                  
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "We cannot find an account with that UserName.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Wrong Password.");
                }
                else
                {
                    ModelState.AddModelError("", "Dang nhap khong dung.");
                }
            }
            return View("Login");
        }
    }
}