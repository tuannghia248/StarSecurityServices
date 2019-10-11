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
            var query = db.Departments.ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.departmentList = list;
        }

        void ContractDropDownList()
        {
            var query = db.Contracts.Where(c => c.status == "Hold" || c.status == "Active").Select(c => new { id = c.id, data = $"{c.code} - {c.Service.name} - {c.Client.name}" }).ToList();
            SelectList list = new SelectList(query, "id", "data");
            ViewBag.contractList = list;
        }

        void ServiceDropDownList()
        {
            var query = db.Services.Where(s => s.status == "Active").ToList();
            SelectList list = new SelectList(query, "id", "name");
            ViewBag.serviceList = list;
        }

        void ClientDropDownList()
        {
            var query = db.Clients.Where(c => c.status == "Waiting" || c.status == "Ongoing").ToList();
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
            var serviceQuery = db.Services.Where(s => s.status == "Active").ToList();
            var clientQuery = db.Clients.Where(c => c.status == "Waiting" || c.status == "Ongoing").ToList();
            SelectList serviceList = new SelectList(serviceQuery, "id", "name", client.service_id);
            SelectList clientList = new SelectList(clientQuery, "id", "name", id);
            ViewBag.serviceList = serviceList;
            ViewBag.clientList = clientList;
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ViewResult EmployeeList()
        {
            var employees = db.Employees.ToList();
            return View(employees);
        }

        public ActionResult EmployeeInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeInsert(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    return RedirectToAction("EmployeeInsert");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EmployeeDetails(int id)
        {
            var empDetails = db.Employees.Single(e => e.id == id);
            return View(empDetails);
        }

        public ActionResult EmployeeEdit(int id)
        {
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin")
            {
                DepartmentDropDownList();
                AccountDropDownList();
                ContractDropDownList();
                var empdetails = db.Employees.Single(e => e.id == id);
                return View(empdetails);
            }
            else
            {
                return RedirectToAction("EmployeeInsert");
            }
        }

        [HttpPost]
        public ActionResult EmployeeEdit(int id, Employee employee)
        {
            DepartmentDropDownList();
            AccountDropDownList();
            ContractDropDownList();
            try
            {
                if (ModelState.IsValid)
                {
                    Employee empToEdit = db.Employees.Single(e => e.id == id);
                    empToEdit.name = employee.name;
                    empToEdit.address = employee.address;
                    empToEdit.phone = employee.phone;
                    empToEdit.email = employee.email;
                    empToEdit.birthday = employee.birthday;
                    empToEdit.position = employee.position;
                    empToEdit.salary = employee.salary;
                    empToEdit.image = employee.image;
                    empToEdit.qualification = employee.qualification;
                    empToEdit.depantment_id = employee.depantment_id;
                    empToEdit.account_id = employee.account_id;
                    empToEdit.contract_id = employee.contract_id;
                    empToEdit.join_at = employee.join_at;
                    empToEdit.resign_at = employee.resign_at;
                    if (employee.contract_id != null)
                    {
                        empToEdit.status = "Active";
                    }
                    if (employee.contract_id == null && employee.resign_at == null)
                    {
                        empToEdit.status = "Standby";
                    }
                    if (employee.contract_id == null && employee.resign_at != null)
                    {
                        empToEdit.status = "Resign";
                    }
                    db.SubmitChanges();
                    return RedirectToAction("EmployeeInsert");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult EmployeeDelete(int id)
        {
            var empToDelete = db.Employees.Single(e => e.id == id);
            db.Employees.DeleteOnSubmit(empToDelete);
            db.SubmitChanges();
            return RedirectToAction("EmployeeInsert");
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
                contractToEdit.price = contract.price;
                contractToEdit.service_id = contract.service_id;
                contractToEdit.client_id = contract.client_id;
                contractToEdit.created_at = contract.created_at;
                contractToEdit.end_at = contract.end_at;
                contractToEdit.status = contract.status;
                if (contract.status == "Complete" || contract.status == "Null")
                {
                    var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
                    employeeOfContract.ForEach(e =>
                    {
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

        [HttpDelete]
        public ActionResult ContractDelete(int id)
        {
            var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
            employeeOfContract.ForEach(e =>
            {
                e.contract_id = null;
                e.status = "Standby";
            });
            var contractToDelete = db.Contracts.Single(c => c.id == id);
            db.Contracts.DeleteOnSubmit(contractToDelete);
            db.SubmitChanges();
            return RedirectToAction("ContractList");
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

        public ActionResult ClientDetails(int id)
        {
            var clientDetails = db.Clients.Single(c => c.id == id);
            return View(clientDetails);
        }

        public ActionResult ClientEdit(int id)
        {
            ServiceDropDownList();
            var clientDetails = db.Clients.Single(c => c.id == id);
            return View(clientDetails);
        }

        [HttpPost]
        public ActionResult ClientEdit(int id, Client client)
        {
            ServiceDropDownList();
            try
            {
                Client clientToEdit = db.Clients.Single(c => c.id == id);
                clientToEdit.name = client.name;
                clientToEdit.address = client.address;
                clientToEdit.phone = client.phone;
                clientToEdit.email = client.email;
                clientToEdit.service_id = client.service_id;
                clientToEdit.description = client.description;
                clientToEdit.status = client.status;
                db.SubmitChanges();
                return RedirectToAction("ClientList");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult ClientDelete(int id)
        {
            var contractOfClient = db.Contracts.Where(c => c.client_id == id).ToList();
            contractOfClient.ForEach(c =>
            {
                var employeeOfContract = db.Employees.Where(e => e.contract_id == c.id).ToList();
                employeeOfContract.ForEach(e =>
                {
                    e.contract_id = null;
                    e.status = "Standby";
                });
            });
            db.Contracts.DeleteAllOnSubmit(contractOfClient);
            var clientToDelete = db.Clients.Single(c => c.id == id);
            db.Clients.DeleteOnSubmit(clientToDelete);
            db.SubmitChanges();
            return RedirectToAction("ClientList");
        }

        public ActionResult ServiceList()
        {
            var services = db.Services.ToList();
            return View(services);
        }

        public ActionResult ServiceInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ServiceInsert(Service service)
        {
            try
            {
                db.Services.InsertOnSubmit(service);
                db.SubmitChanges();
                return RedirectToAction("ServiceList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ServiceDetails(int id)
        {
            var serviceDetails = db.Services.Single(s => s.id == id);
            return View(serviceDetails);
        }

        public ActionResult ServiceEdit(int id)
        {
            var serviceDetails = db.Services.Single(s => s.id == id);
            return View(serviceDetails);
        }

        [HttpPost]
        public ActionResult ServiceEdit(int id, Service service)
        {
            try
            {
                Service serviceToEdit = db.Services.Single(s => s.id == id);
                serviceToEdit.name = service.name;
                serviceToEdit.description = service.description;
                serviceToEdit.image = service.image;
                serviceToEdit.status = service.status;
                db.SubmitChanges();
                return RedirectToAction("ServiceList");
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult ServiceDelete(int id)
        {
            var clientOfService = db.Clients.Where(c => c.service_id == id).ToList();
            clientOfService.ForEach(c =>
            {
                var contractOfClient = db.Contracts.Where(d => d.client_id == c.id).ToList();
                contractOfClient.ForEach(d =>
                {
                    var employeeOfContract = db.Employees.Where(e => e.contract_id == d.id).ToList();
                    employeeOfContract.ForEach(e =>
                    {
                        e.contract_id = null;
                        e.status = "Standby";
                    });
                });
                db.Contracts.DeleteAllOnSubmit(contractOfClient);
            });
            db.Clients.DeleteAllOnSubmit(clientOfService);
            var serviceToDelete = db.Services.Single(s => s.id == id);
            db.Services.DeleteOnSubmit(serviceToDelete);
            db.SubmitChanges();
            return RedirectToAction("ServiceList");
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
                vacancyToEdit.description = vacancy.description;
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