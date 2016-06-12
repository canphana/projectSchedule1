using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using projectSchedule1.Models;
using projectSchedule1.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Mvc.Rendering;
using Sakura.AspNet;
using System.IO;
using Microsoft.Net.Http.Headers;
using Sakura.AspNet.Mvc.PagedList;
using System.Data.SqlClient;

namespace projectSchedule1.Controllers
{
    public class AdminController : Controller
    {

        //private readonly IHostingEnvironment _hostingEnvironment;
        //public AdminController(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        [FromServices]
        public AppDbContext appContext { get; set; }

        public IActionResult AdminDetails()
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                try
                {
                    int id = (int)HttpContext.Session.GetInt32("UserId");
                    Employee employee = appContext.Employees.Include(e => e.Files).SingleOrDefault(e => e.EmployeeId == id);

                    return View(employee);
                }
                catch
                {
                    return View();
                }
            } else
            {
                return HttpNotFound();
            }
        }

        public async Task<ActionResult> EditDetails(int id, int selected = -1)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                try
                {
                    Employee employee = await appContext.Employees.Include(e => e.Files).SingleOrDefaultAsync(e => e.EmployeeId == id);

                    ViewBag.adminStatus = appContext.Statuses.ToList()
                      .OrderBy(status => status.StatusName)
                       .Select(status => new SelectListItem
                       {
                           Text = String.Format("{0}", status.StatusName),
                           Value = status.StatusId.ToString(),
                           Selected = status.StatusId == selected
                       });
                    return View(employee);
                }
                catch
                {
                    return RedirectToAction("AdminDetails");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(int id, [Bind("EmployeeFirstName", "EmployeeLastName", "EmployeeAddr", "EmployeeEmail", "EmployeePhoneNo", "StatusId", "EmployeeBirthDate")]Employee employee, IFormFile upload)
        {
            var temp = appContext.Employees.AsNoTracking().Include(e => e.Files).First(e => e.EmployeeId == id);
            try
            {

                employee.EmployeeId = id;
                employee.Files = temp.Files;
                if (upload != null && upload.Length > 0)
                {
                    if (employee.Files != null)
                    {
                        appContext.Files.Remove(employee.Files.First(f => f.FileType == FileType.Avatar));
                    }
                    var avatar = new Models.File
                    {
                        FileName = ContentDispositionHeaderValue.Parse(upload.ContentDisposition).FileName.Trim('"'),
                        FileType = FileType.Avatar,
                        ContentType = upload.ContentType,
                        EmployeeId = id
                    };
                    using (var reader = new BinaryReader(upload.OpenReadStream()))
                    {
                        avatar.Content = reader.ReadBytes((int)upload.Length);
                    }
                    employee.Files = new List<Models.File> { avatar };
                }
                appContext.Entry(temp).Context.Update(employee);
                appContext.Employees.Attach(employee);
                appContext.Entry(employee).State = EntityState.Modified;
                await appContext.SaveChangesAsync();
                return RedirectToAction("AdminDetails");

            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Unable to save change.");
                return View(employee);
            }

        }

        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModels cpm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var account = await appContext.LoginAcounts
                        .SingleOrDefaultAsync(la => la.LoginAccountPassword == cpm.OldPassword);
                    if (account != null)
                    {
                        account.LoginAccountPassword = cpm.ConfirmNewPassword;
                        appContext.LoginAcounts.Attach(account);
                        await appContext.SaveChangesAsync();
                        return RedirectToAction("AdminDetails");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "There is a problem to change your password");
            }
            return View(cpm);
        }

        public IActionResult CreateProject(int selected1 = -1, int selected2 = -1)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                ViewBag.statusItems = appContext.Statuses.ToList()
                    .OrderBy(status => status.StatusName)
                     .Select(status => new SelectListItem
                     {
                         Text = String.Format("{0}", status.StatusName),
                         Value = status.StatusId.ToString(),
                         Selected = status.StatusId == selected1
                     });

                ViewBag.clientItems = appContext.Clients.ToList()
                   .OrderBy(client => client.ClientName)
                    .Select(client => new SelectListItem
                    {
                        Text = String.Format("{0}", client.ClientName),
                        Value = client.ClientId.ToString(),
                        Selected = client.ClientId == selected2
                    });
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject([Bind("ClientId", "ProjectDesc", "ProjectName", "ProjectEndDate", "ProjectStartDate", "StatusId")]Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appContext.Projects.Add(project);
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("Project");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(project);

        }
        public IActionResult Project(int? page, string searchString, string sortOrder, string currentSort, string currentFilter)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
                ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";
                ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
                ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "status";
                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "client_desc" : "client";
                var projectList = from p in appContext.Projects.Include(p => p.Status).Include(p => p.Client) select p;
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                if (!String.IsNullOrEmpty(searchString))
                {
                    projectList = projectList.Where(p => p.ProjectName.Contains(searchString));

                }
                switch (sortOrder)
                {
                    case "Name_desc":
                        projectList = projectList.OrderByDescending(p => p.ProjectName);
                        break;
                    case "EndDate":
                        projectList = projectList.OrderBy(p => p.ProjectEndDate);
                        break;
                    case "endDate_desc":
                        projectList = projectList.OrderByDescending(p => p.ProjectEndDate);
                        break;
                    case "StartDate":
                        projectList = projectList.OrderBy(p => p.ProjectStartDate);
                        break;
                    case "startDate_desc":
                        projectList = projectList.OrderByDescending(p => p.ProjectStartDate);
                        break;
                    case "status":
                        projectList = projectList.OrderBy(p => p.Status.StatusName);
                        break;
                    case "status_desc":
                        projectList = projectList.OrderByDescending(p => p.Status.StatusName);
                        break;
                    case "client":
                        projectList = projectList.OrderBy(p => p.Client.ClientName);
                        break;
                    case "client_desc":
                        projectList = projectList.OrderByDescending(p => p.Client.ClientName);
                        break;
                    default:
                        projectList = projectList.OrderBy(p => p.ProjectName);
                        break;
                }

                if (projectList != null)
                {
                    var pageNumber = (page ?? 1);
                    var pageSize = 5;
                    return View(projectList.ToPagedList(pageSize, pageNumber));
                }
                else
                {

                    return View();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        public IActionResult CreateEmployee(int selected = -1)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                ViewBag.Items = appContext.Statuses.ToList()
                .OrderBy(status => status.StatusName)
                 .Select(status => new SelectListItem
                 {
                     Text = String.Format("{0}", status.StatusName),
                     Value = status.StatusId.ToString(),
                     Selected = status.StatusId == selected
                 });
                return View();
            }
            else
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee([Bind("EmployeeFirstName", "EmployeeLastName", "EmployeeEmail", "EmployeeAddr", "EmployeeBirthDate", "EmployeePhoneNo", "StatusId")] Employee employee, IFormFile upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.Length > 0)
                    {
                        if (employee.Files != null)
                        {
                            appContext.Files.Remove(employee.Files.First(f => f.FileType == FileType.Avatar));
                        }
                        var avatar = new Models.File
                        {
                            FileName = ContentDispositionHeaderValue.Parse(upload.ContentDisposition).FileName.Trim('"'),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType,
                        };
                        using (var reader = new BinaryReader(upload.OpenReadStream()))
                        {
                            avatar.Content = reader.ReadBytes((int)upload.Length);
                        }
                        employee.Files = new List<Models.File> { avatar };
                    }
                    appContext.Employees.Add(employee);
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("Employee");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(employee);
        }
        public IActionResult Employee(string searchString, string sortOrder, int? page, string currentSort, string currentFilter)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                try
                {
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
                    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                    ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "firstName";
                    ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "status";
                    var employeeList = from el in appContext.Employees.Include(el => el.Status) select el;
                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        employeeList = employeeList.Where(el => el.EmployeeLastName.Contains(searchString)
                                               || el.EmployeeFirstName.Contains(searchString));

                    }
                    switch (sortOrder)
                    {
                        case "lastName_desc":
                            employeeList = employeeList.OrderByDescending(el => el.EmployeeLastName);
                            break;
                        case "Date":
                            employeeList = employeeList.OrderBy(el => el.EmployeeBirthDate);
                            break;
                        case "date_desc":
                            employeeList = employeeList.OrderByDescending(el => el.EmployeeBirthDate);
                            break;
                        case "firstname_desc":
                            employeeList = employeeList.OrderByDescending(el => el.EmployeeFirstName);
                            break;
                        case "firstName":
                            employeeList = employeeList.OrderBy(el => el.EmployeeFirstName);
                            break;
                        case "status":
                            employeeList = employeeList.OrderBy(el => el.Status.StatusName);
                            break;
                        case "status_desc":
                            employeeList = employeeList.OrderByDescending(el => el.Status.StatusName);
                            break;
                        default:
                            employeeList = employeeList.OrderBy(el => el.EmployeeLastName);
                            break;
                    }

                    if (employeeList != null)
                    {
                        var pageNumber = (page ?? 1);
                        var pageSize = 5;
                        var pagedEmployeeList = employeeList.ToPagedList(pageSize, pageNumber);
                        return View(pagedEmployeeList);
                    }
                    else
                    {

                        return View();
                    }
                }
                catch (Exception)
                {
                    return View();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        public async Task<IActionResult> DeleteEmployee(bool confirm, int id)
        {
            try
            {
                if (confirm)
                {
                    Employee employee = await appContext.Employees.FirstAsync(e => e.EmployeeId == id);
                    appContext.Remove(employee);
                    await appContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Employee");

            }
            return RedirectToAction("Employee");
        }

        public IActionResult LoginAccount(string searchString, int? page)
        {

            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                var accountList = new List<AccountModels>();
                try
                {
                    //var loginAccountList = appContext.LoginAcounts
                    //    .Include(act => act.AccountType)
                    //    .Include(e => e.Employee)
                    //    .ToList();
                    var loginAccountList = from la
                                           in appContext.LoginAcounts.Include(act => act.AccountType).Include(e => e.Employee)
                                           select la;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        loginAccountList = loginAccountList.Where(la => la.LoginAccountName.Contains(searchString));

                    }
                    foreach (var item in loginAccountList)
                    {
                        var accountListItem = new AccountModels()
                        {
                            AccountId = item.LoginAccountId,
                            AccountName = item.LoginAccountName,
                            AccountType = item.AccountType.AccountTypeName,
                            AccountEmployee = item.Employee.EmployeeFirstName + " " + item.Employee.EmployeeLastName

                        };
                        accountList.Add(accountListItem);
                    }
                    var pageSize = 5;
                    var pageNumber = (page ?? 1);
                    return View(accountList.ToPagedList(pageSize, pageNumber));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        public IActionResult CreateAccount(int selected1 = -1, int selected2 = -1)
        {
            ViewBag.accountType = appContext.AccountTypes.ToList()
                .OrderBy(act => act.AccountTypeName)
                 .Select(act => new SelectListItem
                 {
                     Text = String.Format("{0}", act.AccountTypeName),
                     Value = act.AccountTypeId.ToString(),
                     Selected = act.AccountTypeId == selected1
                 });

            ViewBag.employee = appContext.Employees.ToList()
              .OrderBy(e => e.EmployeeLastName)
               .Select(e => new SelectListItem
               {
                   Text = String.Format("{0}", e.EmployeeFirstName + " " + e.EmployeeLastName),
                   Value = e.EmployeeId.ToString(),
                   Selected = e.EmployeeId == selected2
               });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount([Bind("AccountTypeId", "EmployeeId", "LoginAccountName", "LoginAccountPassword")] LoginAccount loginAccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appContext.LoginAcounts.Add(loginAccount);
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("LoginAccount");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            return View(loginAccount);
        }
        public async Task<IActionResult> EditAccountDetails(int id, int selected1 = -1, int selected2 = -1)
        {
            try
            {
                LoginAccount loginAccount = await appContext.LoginAcounts
                    .Include(la => la.AccountType)
                    .SingleOrDefaultAsync(la => la.LoginAccountId == id);

                ViewBag.accountType = appContext.AccountTypes.ToList()
                  .OrderBy(act => act.AccountTypeName)
                   .Select(act => new SelectListItem
                   {
                       Text = String.Format("{0}", act.AccountTypeName),
                       Value = act.AccountTypeId.ToString(),
                       Selected = act.AccountTypeId == selected1
                   });

                ViewBag.employee = appContext.Employees.ToList()
                  .OrderBy(e => e.EmployeeLastName)
                   .Select(e => new SelectListItem
                   {
                       Text = String.Format("{0}", e.EmployeeFirstName + e.EmployeeLastName),
                       Value = e.EmployeeId.ToString(),
                       Selected = e.EmployeeId == selected2
                   });
                return View(loginAccount);
            }
            catch (Exception)
            {
                return RedirectToAction("LoginAccount");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAcountDetails(int id, [Bind("AccountTypeId", "EmployeeId", "LoginAccountName", "LoginAccountPassword")] LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    loginAccount.LoginAccountId = id;
                    appContext.LoginAcounts.Attach(loginAccount);
                    appContext.Entry(loginAccount).State = EntityState.Modified;
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("LoginAccount");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Unable to save changes.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Unable to save changes.");
                return View();
            }
        }

        public async Task<IActionResult> DeleteLoginAccount(int id, bool confirm)
        {
            try
            {
                if (confirm)
                {
                    LoginAccount loginAccount = await appContext.LoginAcounts.FirstAsync(c => c.LoginAccountId == id);
                    appContext.Remove(loginAccount);
                    await appContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("LoginAccount");
            }
            return RedirectToAction("LoginAccount");
        }
        public IActionResult Client(int? page, string searchString)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                var clientList = from cl in appContext.Clients select cl;
                if (!String.IsNullOrEmpty(searchString))
                {
                    clientList = clientList.Where(cl => cl.ClientName.Contains(searchString));

                }
                var pageSize = 5;
                var pageNumber = (page ?? 1);
                return View(clientList.ToPagedList(pageSize, pageNumber));
            }
            else
            {
                return HttpNotFound();
            }
        }

        public IActionResult CreateClient()
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient([Bind("ClientName", "ClientAddress", "ClientEmail", "ClientPhoneNo")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appContext.Clients.Add(client);
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("Client");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Unable to save change.");
            }
            return View();
        }


        public async Task<IActionResult> EmployeeDetails(int id)
        {
            try
            {
                var employeeDetails = await appContext.Employees.Include(e => e.Files).FirstOrDefaultAsync(e => e.EmployeeId == id);
                return View(employeeDetails);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<IActionResult> ProjectDetails(int id)
        {
            try
            {
                var employeeDetails = await appContext.Projects.Include(p => p.Client).Include(p=>p.Status).FirstOrDefaultAsync(p => p.ProjectId == id);
                if (employeeDetails != null)
                {
                    return View(employeeDetails);
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<IActionResult> EditProject(int id, int selected1 = -1, int selected2 = -1)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("Admin"))
            {
                try
                {
                    Project project = await appContext.Projects
                        .SingleOrDefaultAsync(p => p.ProjectId == id);
                    ViewBag.statusItems = appContext.Statuses.ToList()
                        .OrderBy(status => status.StatusName)
                         .Select(status => new SelectListItem
                         {
                             Text = String.Format("{0}", status.StatusName),
                             Value = status.StatusId.ToString(),
                             Selected = status.StatusId == selected1
                         });

                    ViewBag.clientItems = appContext.Clients.ToList()
                       .OrderBy(client => client.ClientName)
                        .Select(client => new SelectListItem
                        {
                            Text = String.Format("{0}", client.ClientName),
                            Value = client.ClientId.ToString(),
                            Selected = client.ClientId == selected2
                        });
                    return View(project);
                }
                catch (Exception)
                {
                    return AdminDetails();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(int id, [Bind("StatusId", "ProjectName", "ProjectDesc", "ProjectEndDate", "ClientId", "ProjectStartDate")]Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    project.ProjectId = id;
                    appContext.Projects.Attach(project);
                    appContext.Entry(project).State = EntityState.Modified;
                    await appContext.SaveChangesAsync();
                    return RedirectToAction("ProjectDetails");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Unable to save changes.");
                    return View();
                }
            }
            else {
                ModelState.AddModelError(String.Empty, "Unable to save changes.");
                return View();
            }
        }
        public async Task<IActionResult> DeleteProject(int id, bool confirm)
        {
            try {
                if (confirm)
                {
                    Project project = await appContext.Projects.FirstAsync(p => p.ProjectId == id);
                    appContext.Remove(project);
                    await appContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Project");
            }
            return RedirectToAction("Project");
        }

        public async Task<IActionResult> DeleteClient(int id, bool confirm)
        {
            try
            {
                if (confirm)
                {
                    Client client = await appContext.Clients.FirstAsync(c => c.ClientId == id);
                    appContext.Remove(client);
                    await appContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Client");
            }
            return RedirectToAction("Client");
        }


        public IActionResult AddProjectReport(int id)
        {
            int employeeId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.employeeId = employeeId;
            ViewBag.projectId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProjectReport([Bind("EmployeeId", "ProjectId", "ProjectReportDesc")]ProjectReport projectReport, ICollection<IFormFile> uploadFiles  )
        {
            if (ModelState.IsValid)
            {
                
                int projectId = projectReport.ProjectId;
                projectReport.ProjectReportDate = DateTime.Today;
                foreach(var upload in uploadFiles)
                {
                    if (upload != null && upload.Length > 0)
                    {
                        var reportFile = new Models.File
                        {
                            FileName = ContentDispositionHeaderValue.Parse(upload.ContentDisposition).FileName.Trim('"'),
                            FileType = FileType.Report,
                            ContentType = upload.ContentType,
                        };
                        using (var reader = new BinaryReader(upload.OpenReadStream()))
                        {
                            reportFile.Content = reader.ReadBytes((int)upload.Length);
                        }
                        if (projectReport.Files == null)
                        {
                            projectReport.Files = new List<Models.File> { reportFile };
                        }
                        else
                        {
                            projectReport.Files.Add(reportFile);
                        }
                    }
                }
                appContext.ProjectReports.Add(projectReport);
                await appContext.SaveChangesAsync();
                var returnProjectModel = await appContext.Projects.Include(p => p.Client).Include(p => p.Status).FirstOrDefaultAsync(p => p.ProjectId == projectId);
                return View("ProjectDetails", returnProjectModel);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Unable to save changes.");
            }
            return View();
        }

        public async Task<IActionResult> ProjectTaskDetails(int taskId)
        {
            var projectTasks = await appContext.ProjectTasks
                                        .Include(pt => pt.Status)
                                        .Include(pt => pt.Project)
                                        .FirstOrDefaultAsync(pt => pt.TaskId == taskId);
            return View(projectTasks);
        }

        public IActionResult AddEmployeeProject(int projectId, int? page, string searchString)
        {
            var employeeList = from el in appContext.Employees.Include(el => el.Status) select el;
            //HttpContext.Session.SetInt32("ProjectId", projectId);
            if (!String.IsNullOrEmpty(searchString))
            {
                employeeList = employeeList.Where(el => el.EmployeeLastName.Contains(searchString)
                                       || el.EmployeeFirstName.Contains(searchString));

            }
            if (employeeList != null)
            {
                var pageNumber = (page ?? 1);
                var pageSize = 5;
                var pagedEmployeeList = employeeList.ToPagedList(pageSize, pageNumber);
                return View(pagedEmployeeList);
            }
            else
            {

                return View();
            }
        }

        [Route("Admin/AddEmployeeProject/{projectId}")]
        [HttpPost]
        public async Task<IActionResult> AddEmployeeProject(IFormCollection collection, int projectId)
        {
            var employeeIdList = collection["employeeId"].ToList();
            //int projectId = (int)HttpContext.Session.GetInt32("ProjectId");
            //int projectId = Convert.ToInt32(ActionContext.RouteData.Values["projectId"]);
            Project project = await appContext.Projects.Include(p=>p.Client).Include(p=>p.Status).Include(p => p.EmployeeProjects).FirstOrDefaultAsync(p => p.ProjectId == projectId);
            try
            {
                foreach (var item in employeeIdList)
                {
                    int employeeId = Int32.Parse(item);
                    Employee employee = await appContext.Employees.Include(e => e.EmployeeProjects).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
                    EmployeeProject employeeProject = new EmployeeProject
                    {
                        EmployeeId = employeeId,
                        ProjectId = projectId
                    };
                    if (employee.EmployeeProjects != null)
                    {
                        employee.EmployeeProjects.Add(employeeProject);
                    }
                    else
                    {
                        employee.EmployeeProjects = new List<EmployeeProject> { employeeProject };
                    }
                    if (project.EmployeeProjects != null)
                    {
                        project.EmployeeProjects.Add(employeeProject);
                    }
                    else
                    {
                        project.EmployeeProjects = new List<EmployeeProject> { employeeProject };
                    }
                    appContext.Employees.Attach(employee);
                    appContext.Projects.Attach(project);
                    await appContext.SaveChangesAsync();
                }
                //HttpContext.Session.Remove("ProjectId");
                return View("ProjectDetails", project);
            }
            catch(Exception)
            {
                return View();
            }
        }
       
        public IActionResult AddEmployeeTask(int projectId,int taskId, int? page, string searchString)
        {
            try
            {
                var employeeList = from el
                                   in appContext.Employees.Include(el => el.Status)
                                   where el.EmployeeProjects.Any(ep => ep.ProjectId == projectId)
                                   select el;
                //HttpContext.Session.SetInt32("ProjectId", projectId);
                HttpContext.Session.SetInt32("TaskId", taskId);
                if (!String.IsNullOrEmpty(searchString))
                {
                    //employeeList = from el
                    //               in appContext.Employees.Include(el => el.Status).Include(el => el.EmployeeProjects)
                    //               where el.EmployeeProjects.Any(ep => ep.ProjectId == projectId) && ((el.EmployeeLastName.Contains(searchString)) || el.EmployeeFirstName.Contains(searchString))
                    //               select el;
                    employeeList = employeeList.Where(el => el.EmployeeLastName.Contains(searchString)|| el.EmployeeFirstName.Contains(searchString));

                }
                if (employeeList != null)
                {
                    var pageNumber = (page ?? 1);
                    var pageSize = 5;
                    var pagedEmployeeList = employeeList.ToPagedList(pageSize, pageNumber);
                    return View(pagedEmployeeList);
                }
                else
                {

                    return View();
                }
            }
            catch(Exception)
            {
                return View();
            }
        }
       
        [HttpPost]
        public async Task<IActionResult> AddEmployeeTask(IFormCollection collection)
        {
            var employeeIdList = collection["employeeId"].ToList();
            //int projectId = (int)HttpContext.Session.GetInt32("ProjectId");
            int taskId = (int)HttpContext.Session.GetInt32("TaskId");
            ProjectTask projectTask = await appContext.ProjectTasks.Include(pt => pt.Status).Include(p => p.EmployeeTasks).FirstOrDefaultAsync(p => p.TaskId == taskId);
            try
            {
                foreach (var item in employeeIdList)
                {
                    int employeeId = Int32.Parse(item);
                    Employee employee = await appContext.Employees.Include(e => e.EmployeeTasks).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
                    EmployeeTask employeeTask = new EmployeeTask
                    {
                        EmployeeId = employeeId,
                        TaskId = taskId
                    };
                    if (employee.EmployeeTasks != null)
                    {
                        employee.EmployeeTasks.Add(employeeTask);
                    }
                    else
                    {
                        employee.EmployeeTasks = new List<EmployeeTask> { employeeTask };
                    }
                    if (projectTask.EmployeeTasks != null)
                    {
                        projectTask.EmployeeTasks.Add(employeeTask);
                    }
                    else
                    {
                        projectTask.EmployeeTasks = new List<EmployeeTask> { employeeTask };
                    }
                    appContext.Employees.Attach(employee);
                    appContext.ProjectTasks.Attach(projectTask);
                    await appContext.SaveChangesAsync();
                }
                HttpContext.Session.Remove("TaskId");
                return View("ProjectTaskDetails", projectTask);
            }
            catch (Exception)
            {
                return View();
            }
        }
        public IActionResult AddTask(int projectId, int selected =-1)
        {
            ViewBag.projectId = projectId;
            ViewBag.statusItems = appContext.Statuses.ToList()
            .OrderBy(status => status.StatusName)
             .Select(status => new SelectListItem
             {
                 Text = String.Format("{0}", status.StatusName),
                 Value = status.StatusId.ToString(),
                 Selected = status.StatusId == selected
             });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask([Bind("TaskDesc", "TaskStartDate", "TaskEndDate", "ProjectId", "StatusId")] ProjectTask projectTask)
        {
            if(ModelState.IsValid)
            {
                appContext.ProjectTasks.Add(projectTask);
                await appContext.SaveChangesAsync();
                var returnProjectModel = await appContext.Projects.Include(p => p.Client).Include(p => p.Status).FirstOrDefaultAsync(p => p.ProjectId == projectTask.ProjectId);
                return View("ProjectDetails", returnProjectModel);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Unable to save changes.");
                return View();
            }
        }

    }

   
}
