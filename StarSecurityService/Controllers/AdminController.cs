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

        void ContractDropDownList()
        {
            var query = db.Contracts.Where(c => c.status == "Hold" || c.status == "Active").Select(c => new { id = c.id, data = $"{c.id} - {c.Service.name} - {c.Client.name}" }).ToList();
            SelectList list = new SelectList(query, "id", "data");
            ViewBag.contractList = list;
        }

        void ServiceDropDownList()
        {
            var query = db.Services.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.serviceList = list;
        }

        void ClientDropDownList()
        {
            var query = db.Clients.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.clientList = list;
        }

        void AccountDropDownList()
        {
            var query = db.Accounts.ToList();
            SelectList list = new SelectList(query, "id", "username");
            ViewBag.accountList = list;
        }

        void ClientToContractDDLSelected(int? id)
        {
            var client = db.Clients.Single(c => c.id == id);
            var serviceQuery = db.Services.ToList();
            var clientQuery = db.Clients.ToList();
            SelectList serviceList = new SelectList(serviceQuery, "id", "name", client.service_id);
            SelectList clientList = new SelectList(clientQuery, "id", "name", id);
            ViewBag.serviceList = serviceList;
            ViewBag.clientList = clientList;
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
            ContractDropDownList();
            AccountDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeInsert(Employee employee)
        {
            ContractDropDownList();
            AccountDropDownList();
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
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin")
            {
                ContractDropDownList();
                AccountDropDownList();
                var empdetails = db.Employees.Single(e => e.id == id);
                return View(empdetails);
            }
            else
            {
                return RedirectToAction("EmployeeList");
            }
        }

        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            ContractDropDownList();
            AccountDropDownList();
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

        public PartialViewResult GetEmployeeOfContract(int id)
        {
            var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
            return PartialView("EmployeePartialView", employeeOfContract);
        }

        public ActionResult ContractInsert(int? id)
        {
            if (id == null)
            {
                ServiceDropDownList();
                ClientDropDownList();
                return View();
            }
            else
            {
                ClientToContractDDLSelected(id);
                return View();
            }
        }

        [HttpPost]
        public ActionResult ContractInsert(int? id, Contract contract)
        {
            if (id == null)
            {
                ServiceDropDownList();
                ClientDropDownList();
                try
                {
                    db.Contracts.InsertOnSubmit(contract);
                    var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                    clientCheck.status = "Ongoing";
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ClientToContractDDLSelected(id);
                try
                {
                    db.Contracts.InsertOnSubmit(contract);
                    var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                    clientCheck.status = "Ongoing";
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
                catch
                {
                    return View();
                }
            }
        }

        public ActionResult ContractDetails(int id)
        {
            var contractDetails = db.Contracts.Single(e => e.id == id);
            return View(contractDetails);
        }

        public ActionResult ContractEdit(int id)
        {
            ServiceDropDownList();
            ClientDropDownList();
            var contractDetails = db.Contracts.Single(e => e.id == id);
            return View(contractDetails);
        }

        [HttpPost]
        public ActionResult ContractEdit(int id, Contract contract)
        {
            ServiceDropDownList();
            ClientDropDownList();
            try
            {
                Contract contractToEdit = db.Contracts.Single(e => e.id == id);
                contractToEdit.duration = contract.duration;
                contractToEdit.total = contract.total;
                contractToEdit.service_id = contract.service_id;
                contractToEdit.client_id = contract.client_id;
                contractToEdit.created_at = contract.created_at;
                contractToEdit.end_at = contract.end_at;
                contractToEdit.status = contract.status;
                if (contract.status == "Complete" || contract.status == "Null")
                {
                    var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
                    employeeOfContract.ForEach(e => {
                        e.contract_id = null;
                        e.status = "Standby";
                    });
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
                else
                {
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
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

        public ActionResult ClientInsert()
        {
            ServiceDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult ClientInsert(Client client)
        {
            ServiceDropDownList();
            try
            {
                db.Clients.InsertOnSubmit(client);
                db.SubmitChanges();
                return RedirectToAction("ClientList");
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

        public ActionResult VacancyList()
        {
            var vacancies = db.Vacancies.ToList();
            return View(vacancies);
        }

        public ActionResult VacancyInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VacancyInsert(Vacancy vacancy)
        {
            try
            {
                db.Vacancies.InsertOnSubmit(vacancy);
                db.SubmitChanges();
                return RedirectToAction("VacancyList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult VacancyDetails(int id)
        {
            var vacancyDetails = db.Vacancies.Single(e => e.id == id);
            return View(vacancyDetails);
        }

        public ActionResult VacancyEdit(int id)
        {
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin" || session.Role == "manager")
            {
                var vacancyDetails = db.Vacancies.Single(e => e.id == id);
                return View(vacancyDetails);
            }
            else
            {
                return RedirectToAction("VacancyList");
            }
        }

        [HttpPost]
        public ActionResult VacancyEdit(int id, Vacancy vacancy)
        {
            try
            {
                Vacancy vacancyToEdit = db.Vacancies.Single(e => e.id == id);
                vacancyToEdit.job = vacancy.job;
                vacancyToEdit.position = vacancy.position;
                vacancyToEdit.quantity = vacancy.quantity;
                vacancyToEdit.deadline = vacancy.deadline;
                db.SubmitChanges();
                return RedirectToAction("VacancyList");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult VacancyDelete(int id)
        {
            var vacancyToDelete = db.Vacancies.Single(e => e.id == id);
            db.Vacancies.DeleteOnSubmit(vacancyToDelete);
            db.SubmitChanges();
            return RedirectToAction("VacancyList");
        }

        public ActionResult AccountList()
        {
            var accounts = db.Accounts.ToList();
            return View(accounts);
        }

        public ActionResult AccountInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountInsert(Account account)
        {
            try
            {
                db.Accounts.InsertOnSubmit(account);
                db.SubmitChanges();
                return RedirectToAction("AccountList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AccountDetails(int id)
        {
            var accountDetails = db.Accounts.Single(e => e.id == id);
            return View(accountDetails);
        }

        public ActionResult AccountEdit(int id)
        {
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin")
            {
                var accountDetails = db.Accounts.Single(e => e.id == id);
                return View(accountDetails);
            }
            else
            {
                return RedirectToAction("AccountList");
            }
            
        }

        [HttpPost]
        public ActionResult AccountEdit(int id, Account account)
        {
            try
            {
                Account accountToEdit = db.Accounts.Single(e => e.id == id);
                accountToEdit.username = account.username;
                accountToEdit.password = account.password;
                accountToEdit.role = account.role;
                db.SubmitChanges();
                return RedirectToAction("AccountList");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult AccountDelete(int id)
        {
            var accountToDelete = db.Accounts.Single(e => e.id == id);
            db.Accounts.DeleteOnSubmit(accountToDelete);
            db.SubmitChanges();
            return RedirectToAction("AccountList");
        }
    }
}