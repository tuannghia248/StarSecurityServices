using StarSecurityService.App_Start;
using StarSecurityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StarSecurityService.Controllers
{
    [SessionTimeout]
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
            var query = db.Contracts.Where(c => c.status == "Active" || c.status == "Pending").Select(c => new { id = c.id, data = $"{c.code} - {c.Service.name} - {c.Client.name}" }).ToList();
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
            var query = db.Clients.Select(c => new { id = c.id, data = $"{c.id} - {c.name} - {c.Service.name} - {c.quantity + " Person(s)"} - {c.duration + " Day(s)"}" }).ToList();
            SelectList list = new SelectList(query, "id", "data");
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
            var clientQuery = db.Clients.Select(c => new { id = c.id, data = $"{c.id} - {c.name} - {c.Service.name} - {c.quantity + " Person(s)"} - {c.duration + " Day(s)"}" }).ToList();
            SelectList serviceList = new SelectList(serviceQuery, "id", "name", client.service_id);
            SelectList clientList = new SelectList(clientQuery, "id", "data", id);
            ViewBag.serviceList = serviceList;
            ViewBag.clientList = clientList;
            ViewBag.quantity = client.quantity;
            ViewBag.duration = client.duration;
            ViewBag.start_at = client.start_at;
            if (client.quantity != null && client.duration != null)
            {
                if (client.service_id == 1)
                {
                    ViewBag.price = 1000000 * client.quantity * client.duration;
                }
                else if (client.service_id == 2)
                {
                    ViewBag.price = 1500000 * client.quantity * client.duration;
                }
                else if (client.service_id == 3)
                {
                    ViewBag.price = 2000000 * client.quantity * client.duration;
                }
                else
                {
                    ViewBag.price = 2500000 * client.quantity * client.duration;
                }
            }
            else
            {
                ViewBag.price = null;
            }
        }

        string SetContractCode(int id)
        {
            string code = "";
            if (id == 1)
            {
                var count = db.Contracts.Where(c => c.service_id == 1).Count();
                if (count > 0)
                {
                    var str = db.Contracts.Where(c => c.service_id == 1).OrderByDescending(c => c.code).Select(a => a.code).First();
                    string digits = new string(str.Where(char.IsDigit).ToArray());
                    int numbers;
                    if (int.TryParse(digits, out numbers))
                    {
                        code += "SE" + (++numbers).ToString("D4");
                    }
                    return code;
                }
                else
                {
                    return "SE0001";
                }
            }
            else if (id == 2)
            {
                var count = db.Contracts.Where(c => c.service_id == 2).Count();
                if (count > 0)
                {
                    var str = db.Contracts.Where(c => c.service_id == 2).OrderByDescending(c => c.code).Select(a => a.code).First();
                    string digits = new string(str.Where(char.IsDigit).ToArray());
                    int numbers;
                    if (int.TryParse(digits, out numbers))
                    {
                        code += "SM" + (++numbers).ToString("D4");
                    }
                    return code;
                }
                else
                {
                    return "SM0001";
                }
            }
            else if (id == 3)
            {
                var count = db.Contracts.Where(c => c.service_id == 3).Count();
                if (count > 0)
                {
                    var str = db.Contracts.Where(c => c.service_id == 3).OrderByDescending(c => c.code).Select(a => a.code).First();
                    string digits = new string(str.Where(char.IsDigit).ToArray());
                    int numbers;
                    if (int.TryParse(digits, out numbers))
                    {
                        code += "PP" + (++numbers).ToString("D4");
                    }
                    return code;
                }
                else
                {
                    return "PP0001";
                }
            }
            else
            {
                var count = db.Contracts.Where(c => c.service_id == 4).Count();
                if (count > 0)
                {
                    var str = db.Contracts.Where(c => c.service_id == 4).OrderByDescending(c => c.code).Select(a => a.code).First();
                    string digits = new string(str.Where(char.IsDigit).ToArray());
                    int numbers;
                    if (int.TryParse(digits, out numbers))
                    {
                        code += "GT" + (++numbers).ToString("D4");
                    }
                    return code;
                }
                else
                {
                    return "GT0001";
                }
            }
        }

        public ActionResult Dashboard()
        {
            var clients = db.Clients.Where(c => c.status == "Waiting").Count();
            var contracts = db.Contracts.Where(c => c.status == "Active").Count();
            var dispatch = db.Employees.Where(e => e.contract_id != null).Count();
            var training = db.Employees.Where(e => e.depantment_id == 4).Count();
            var list = db.Contracts.OrderByDescending(c => c.id).Take(5);
            ViewBag.clients = clients;
            ViewBag.contracts = contracts;
            ViewBag.dispatch = dispatch;
            ViewBag.training = training;
            ViewBag.list = list;
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
            if (session.Role == "admin" || session.Role == "manager")
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

        public ActionResult ContractList(string status)
        {
            if (status == null)
            {
                var contracts = db.Contracts.ToList();
                return View(contracts);
            }
            else if (status == "Pending")
            {
                var contracts = db.Contracts.Where(c => c.status == "Pending").ToList();
                return View(contracts);
            }
            else if (status == "Active")
            {
                var contracts = db.Contracts.Where(c => c.status == "Active").ToList();
                return View(contracts);
            }
            else if (status == "Complete")
            {
                var contracts = db.Contracts.Where(c => c.status == "Complete").ToList();
                return View(contracts);
            }
            else
            {
                var contracts = db.Contracts.Where(c => c.status == "Null").ToList();
                return View(contracts);
            }
        }

        public PartialViewResult GetEmployeeOfContract(int id)
        {
            var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
            return PartialView("EmployeePartialView", employeeOfContract);
        }

        public ActionResult ContractInsert(int? id)
        {
            var list = db.Contracts.OrderByDescending(c => c.id).Take(5);
            ViewBag.list = list;
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
            var list = db.Contracts.OrderByDescending(c => c.id).Take(5);
            ViewBag.list = list;
            if (id == null)
            {
                ServiceDropDownList();
                ClientDropDownList();
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Contracts.InsertOnSubmit(contract);
                        contract.code = SetContractCode(contract.service_id);
                        var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                        clientCheck.status = "Ongoing";
                        db.SubmitChanges();
                        return RedirectToAction("ContractInsert");
                    }
                    return View();
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
                    if (ModelState.IsValid)
                    {
                        db.Contracts.InsertOnSubmit(contract);
                        contract.code = SetContractCode(contract.service_id);
                        var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                        clientCheck.status = "Ongoing";
                        db.SubmitChanges();
                        return RedirectToAction("ContractInsert");
                    }
                    return View();
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
                if (ModelState.IsValid)
                {
                    Contract contractToEdit = db.Contracts.Single(e => e.id == id);
                    contractToEdit.service_id = contract.service_id;
                    contractToEdit.client_id = contract.client_id;
                    contractToEdit.quantity = contract.quantity;
                    contractToEdit.duration = contract.duration;
                    contractToEdit.price = contract.price;
                    contractToEdit.start_at = contract.start_at;
                    contractToEdit.end_at = contract.end_at;
                    contractToEdit.status = contract.status;
                    contractToEdit.updated_at = DateTime.Now.ToString("MM/dd/yyyy");
                    if (contract.status == "Complete" || contract.status == "Null")
                    {
                        var employeeOfContract = db.Employees.Where(e => e.contract_id == id).ToList();
                        employeeOfContract.ForEach(e =>
                        {
                            e.contract_id = null;
                            e.status = "Standby";
                        });
                        var contractOfClient = db.Contracts.Where(c => c.client_id == contract.client_id).ToList();
                        var countContractActive = contractOfClient.Where(c => c.status == "Active").Count();
                        if (countContractActive == 0)
                        {
                            var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                            clientCheck.status = "Fulfill";
                            db.SubmitChanges();
                            return RedirectToAction("ContractList");
                        }
                        else
                        {
                            db.SubmitChanges();
                            return RedirectToAction("ContractList");
                        }
                    }
                    else
                    {
                        var clientCheck = db.Clients.Single(c => c.id == contract.client_id);
                        if (clientCheck.status == "Fulfill")
                        {
                            clientCheck.status = "Ongoing";
                            db.SubmitChanges();
                            return RedirectToAction("ContractList");
                        }
                        else
                        {
                            db.SubmitChanges();
                            return RedirectToAction("ContractList");
                        }
                    }
                }
                return View();
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
            var countContract = db.Contracts.Where(c => c.client_id == contractToDelete.client_id).Count();
            if (countContract == 1)
            {
                var clientCheck = db.Clients.Single(c => c.id == contractToDelete.client_id);
                if (clientCheck.status == "Ongoing")
                {
                    clientCheck.status = "Waiting";
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
                else
                {
                    db.SubmitChanges();
                    return RedirectToAction("ContractList");
                }
            }
            else
            {
                db.SubmitChanges();
                return RedirectToAction("ContractList");
            }
        }

        public ActionResult ClientList(string status)
        {
            if (status == null)
            {
                var clients = db.Clients.ToList();
                return View(clients);
            }
            else if (status == "Waiting")
            {
                var clients = db.Clients.Where(c => c.status == "Waiting").ToList();
                return View(clients);
            }
            else if (status == "Ongoing")
            {
                var clients = db.Clients.Where(c => c.status == "Ongoing").ToList();
                return View(clients);
            }
            else
            {
                var clients = db.Clients.Where(c => c.status == "Fulfill").ToList();
                return View(clients);
            }
        }

        public ActionResult ClientInsert()
        {
            var list = db.Clients.OrderByDescending(c => c.id).Take(5);
            ViewBag.list = list;
            ServiceDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult ClientInsert(Client client)
        {
            var list = db.Clients.OrderByDescending(c => c.id).Take(5);
            ViewBag.list = list;
            ServiceDropDownList();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Clients.InsertOnSubmit(client);
                    db.SubmitChanges();
                    return RedirectToAction("ClientInsert");
                }
                return View();
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
                if (ModelState.IsValid)
                {
                    Client clientToEdit = db.Clients.Single(c => c.id == id);
                    clientToEdit.name = client.name;
                    clientToEdit.address = client.address;
                    clientToEdit.phone = client.phone;
                    clientToEdit.email = client.email;
                    clientToEdit.service_id = client.service_id;
                    clientToEdit.quantity = client.quantity;
                    clientToEdit.duration = client.duration;
                    clientToEdit.start_at = client.start_at;
                    clientToEdit.description = client.description;
                    clientToEdit.updated_at = DateTime.Now.ToString("MM/dd/yyyy");
                    db.SubmitChanges();
                    return RedirectToAction("ClientList");
                }
                return View();
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

        public ViewResult ServiceList()
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
                if (ModelState.IsValid)
                {
                    db.Services.InsertOnSubmit(service);
                    db.SubmitChanges();
                    return RedirectToAction("ServiceInsert");
                }
                return View();
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
                if (ModelState.IsValid)
                {
                    Service serviceToEdit = db.Services.Single(s => s.id == id);
                    serviceToEdit.name = service.name;
                    serviceToEdit.description = service.description;
                    serviceToEdit.image = service.image;
                    serviceToEdit.status = service.status;
                    serviceToEdit.price = service.price;
                    db.SubmitChanges();
                    return RedirectToAction("ServiceInsert");
                }
                return View();
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
            return RedirectToAction("ServiceInsert");
        }

        public ViewResult DepartmentList()
        {
            var departments = db.Departments.ToList();
            return View(departments);
        }

        public ActionResult DepartmentInsert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DepartmentInsert(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Departments.InsertOnSubmit(department);
                    db.SubmitChanges();
                    return RedirectToAction("DepartmentInsert");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DepartmentEdit(int id)
        {
            var departmentDetails = db.Departments.Single(d => d.id == id);
            return View(departmentDetails);
        }

        [HttpPost]
        public ActionResult DepartmentEdit(int id, Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Department departmentToEdit = db.Departments.Single(d => d.id == id);
                    departmentToEdit.name = department.name;
                    departmentToEdit.description = department.description;
                    db.SubmitChanges();
                    return RedirectToAction("DepartmentInsert");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult DepartmentDelete(int id)
        {
            var departmentToDelete = db.Departments.Single(d => d.id == id);
            db.Departments.DeleteOnSubmit(departmentToDelete);
            db.SubmitChanges();
            return RedirectToAction("DepartmentInsert");
        }

        public ViewResult VacancyList()
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
                if (ModelState.IsValid)
                {
                    db.Vacancies.InsertOnSubmit(vacancy);
                    db.SubmitChanges();
                    return RedirectToAction("VacancyInsert");
                }
                return View();
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
                return RedirectToAction("VacancyInsert");
            }
        }

        [HttpPost]
        public ActionResult VacancyEdit(int id, Vacancy vacancy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Vacancy vacancyToEdit = db.Vacancies.Single(e => e.id == id);
                    vacancyToEdit.job = vacancy.job;
                    vacancyToEdit.description = vacancy.description;
                    vacancyToEdit.quantity = vacancy.quantity;
                    vacancyToEdit.requirement = vacancy.requirement;
                    vacancyToEdit.salary = vacancy.salary;
                    vacancyToEdit.deadline = vacancy.deadline;
                    db.SubmitChanges();
                    return RedirectToAction("VacancyInsert");
                }
                return View();
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
            return RedirectToAction("VacancyInsert");
        }

        public ViewResult AccountList()
        {
            var accounts = db.Accounts.ToList();
            return View(accounts);
        }

        public ActionResult AccountInsert()
        {
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin" || session.Role == "manager")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public ActionResult AccountInsert(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Accounts.Any(a => a.username == account.username))
                    {
                        ModelState.AddModelError("UsernameExits", "Username already exists.");
                    }
                    else
                    {
                        db.Accounts.InsertOnSubmit(account);
                        db.SubmitChanges();
                        return RedirectToAction("AccountInsert");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AccountDetails(int id)
        {
            var session = (StarSecurityService.Common.UserLogin)Session[StarSecurityService.Common.CommonConstants.USER_SESSION];
            if (session.Role == "admin" || session.Role == "manager")
            {
                var accountDetails = db.Accounts.Single(e => e.id == id);
                return View(accountDetails);
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
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
                return RedirectToAction("AccountInsert");
            }
        }

        [HttpPost]
        public ActionResult AccountEdit(int id, Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account accountToEdit = db.Accounts.Single(e => e.id == id);
                    accountToEdit.username = account.username;
                    accountToEdit.password = account.password;
                    accountToEdit.role = account.role;
                    db.SubmitChanges();
                    return RedirectToAction("AccountInsert");
                }
                return View();
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
            return RedirectToAction("AccountInsert");
        }
    }
}