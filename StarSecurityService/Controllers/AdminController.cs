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

        void DepartmentList()
        {
            Department dep = new Department();
            var query = db.Departments.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.deplist = list;
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
            DepartmentList();
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeInsert(Employee employee)
        {
            DepartmentList();
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
            DepartmentList();
            var empdetails = db.Employees.Single(e => e.id == id);
            return View(empdetails);
        }

        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            DepartmentList();
            try
            {
                Employee emptoedit = db.Employees.Single(e => e.id == id);
                emptoedit.name = employee.name;
                emptoedit.address = employee.address;
                emptoedit.number = employee.number;
                emptoedit.birthday = employee.birthday;
                emptoedit.position = employee.position;
                emptoedit.image = employee.image;
                emptoedit.qualification = employee.qualification;
                emptoedit.achievement = employee.achievement;
                emptoedit.depantment_id = employee.depantment_id;
                emptoedit.account_id = employee.account_id;
                emptoedit.contract_id = employee.contract_id;
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
    }
}