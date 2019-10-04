using StarSecurityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StarSecurityService.Controllers
{
    public class AdminController : Controller
    {
        StarSecurityDataDataContext db = new StarSecurityDataDataContext();

        void DepartmentDropDownList()
        {
            Department dep = new Department();
            var query = db.Departments.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.deplist = list;
        }

        void ServiceDropDownList()
        {
            Service ser = new Service();
            var query = db.Services.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.serlist = list;
        }

        void ClientDropDownList()
        {
            Client cli = new Client();
            var query = db.Clients.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.clilist = list;
        }

        void ClientDropDownListSelected(int id)
        {
            Client cli = new Client();
            var query = db.Clients.ToList();
            SelectList list = new SelectList(query, "id", "name", id);
            ViewBag.clilist = list;
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult EmployeeList()
        {
            var employees = db.Employees.ToList();
            return View(employees);
        }

        public ActionResult EmployeeInsert()
        {
            DepartmentDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeInsert(Employee employee)
        {
            DepartmentDropDownList();
            try
            {
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
                return RedirectToAction("EmployeeList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EmployeeDetails(int id)
        {
            var empdetails = db.Employees.Single(e => e.id == id);
            return View(empdetails);
        }

        public ActionResult EmployeeEdit(int id)
        {
            DepartmentDropDownList();
            var empdetails = db.Employees.Single(e => e.id == id);
            return View(empdetails);
        }

        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            DepartmentDropDownList();
            try
            {
                Employee emptoedit = db.Employees.Single(e => e.id == id);
                emptoedit.name = employee.name;
                emptoedit.address = employee.address;
                emptoedit.phone = employee.phone;
                emptoedit.birthday = employee.birthday;
                emptoedit.position = employee.position;
                emptoedit.image = employee.image;
                emptoedit.salary = employee.salary;
                emptoedit.qualification = employee.qualification;
                emptoedit.achievement = employee.achievement;
                emptoedit.depantment_id = employee.depantment_id;
                emptoedit.account_id = employee.account_id;
                emptoedit.contract_id = employee.contract_id;
                emptoedit.status = employee.status;
                db.SubmitChanges();
                return RedirectToAction("EmployeeList");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult EmployeeDelete(int id)
        {
            var emptodelete = db.Employees.Single(e => e.id == id);
            db.Employees.DeleteOnSubmit(emptodelete);
            db.SubmitChanges();
            return RedirectToAction("EmployeeList");
        }

        public ActionResult ContractList()
        {
            var contracts = db.Contracts.ToList();
            return View(contracts);
        }

        public ActionResult ContractInsert()
        {
            ServiceDropDownList();
            ClientDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult ContractInsert(Contract contract)
        {
            ServiceDropDownList();
            ClientDropDownList();
            try
            {
                db.Contracts.InsertOnSubmit(contract);
                db.SubmitChanges();
                return RedirectToAction("ContractList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ClientList()
        {
            var clients = db.Clients.ToList();
            return View(clients);
        }

        public ActionResult ClientToContact(int id)
        {
            ServiceDropDownList();
            ClientDropDownListSelected(id);
            return View();
        }

        [HttpPost]
        public ActionResult ClientToContact(int id, Contract contract)
        {
            ServiceDropDownList();
            ClientDropDownListSelected(id);
            try
            {
                db.Contracts.InsertOnSubmit(contract);
                db.SubmitChanges();
                return RedirectToAction("ContractList");
            }
            catch
            {
                return View();
            }
        }

        //public PartialViewResult GetClient(int id)
        //{
        //    var model = db.Clients.Single(c => c.id == id);
        //    return PartialView("MyPartialView", model);
        //}
    }
}