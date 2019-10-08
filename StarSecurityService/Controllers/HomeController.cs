﻿using StarSecurityService.Models;
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
            try
            {
                cl.status = "waiting";
                data.Clients.InsertOnSubmit(cl);
                data.SubmitChanges();

                return RedirectToAction("Index");
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
    }
}