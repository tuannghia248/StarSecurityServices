using StarSecurityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StarSecurityService.Controllers
{
    public class HomeController : Controller
    {
        StarSecurityDataDataContext data = new StarSecurityDataDataContext();

        void ServiceDropDownList()
        {
            Service sv = new Service();
            var query = data.Services.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.svlist = list;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ServiceDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Client cl)
        {
            //ServiceDropDownList();
            try
            {
                //if (ModelState.IsValid)
                //{
                //    cl.status = "Waiting";
                //    data.Clients.InsertOnSubmit(cl);
                //    data.SubmitChanges();
                //    TempData["Referrer"] = "SaveRegister";
                //    return RedirectToAction("Contact");
                //}
                //return View();

                cl.status = "Waiting";
                data.Clients.InsertOnSubmit(cl);
                data.SubmitChanges();
                TempData["Referrer"] = "SaveRegister";
                return RedirectToAction("Contact");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Service()
        {
            var model = data.Services.ToList();
            return View(model);
        }

        public ActionResult Recruitment()
        {
            var model = data.Vacancies.ToList();
            return View(model);
        }

        public ActionResult Network()
        {
            return View();
        }

        public ActionResult Testimonial()
        {
            return View();
        }
    }
}