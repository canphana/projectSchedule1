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
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace projectSchedule1.Controllers
{
    public class UserController : Controller
    {
        [FromServices]
        public AppDbContext appContext { get; set; }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserDetails()
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
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
            }
            else
            {
                return HttpNotFound();
            }
        }
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
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
                        return RedirectToAction("UserDetails");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "There is a problem to change your password");
            }
            return View(cpm);
        }
        public IActionResult Client(int? page, string searchString)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
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
        public IActionResult Employee(string searchString, string sortOrder, int? page, string currentSort, string currentFilter)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
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
        public IActionResult Project(int? page, string searchString, string sortOrder, string currentSort, string currentFilter)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
            {
                int employeeId =  (int)HttpContext.Session.GetInt32("UserId");
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
                ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";
                ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
                ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "status";
                ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "client_desc" : "client";
                var projectList = from p in appContext.Projects
                                  .Include(p => p.Status)
                                  .Include(p => p.Client)
                                  .Where(p=>p.EmployeeProjects.Any(ep=>ep.EmployeeId==employeeId))select p;
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
                    projectList = projectList.Where(p => p.ProjectName.Contains(searchString)
                                            || p.ProjectName.Contains(searchString));

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

        public async Task<IActionResult> ProjectDetails(int id)
        {
            if (HttpContext.Session.GetString("UserType") != null && HttpContext.Session.GetString("UserType").Equals("User"))
            {
                try
                {
                    var projectDetails = await appContext.Projects.Include(p => p.Client).Include(p=>p.Status).FirstOrDefaultAsync(p => p.ProjectId == id);
                    if (projectDetails != null)
                    {
                        return View(projectDetails);
                    }
                    return View();
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

        public IActionResult AddProjectReport(int id)
        {
            int employeeId = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.employeeId = employeeId;
            ViewBag.projectId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProjectReport([Bind("EmployeeId","ProjectId", "ProjectReportDesc")]ProjectReport projectReport)
        {
            if(ModelState.IsValid)
            {   int projectId = projectReport.ProjectId;
                projectReport.ProjectReportDate = DateTime.Today;
                appContext.ProjectReports.Add(projectReport);
                await appContext.SaveChangesAsync();
                var retunnProjectModel =  await appContext.Projects.Include(p=>p.Client).Include(p=>p.Status).FirstOrDefaultAsync(p=>p.ProjectId==projectId);
                return View("ProjectDetails",retunnProjectModel);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Unable to save changes.");
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

        public async Task<IActionResult> ProjectTaskDetails(int taskId)
        {
            var projectTasks = await appContext.ProjectTasks
                                        .Include(pt => pt.Status)
                                        .Include(pt => pt.Project)
                                        .FirstOrDefaultAsync(pt => pt.TaskId == taskId);
            return View(projectTasks);
        }

        public async Task<IActionResult> EditTaskDetails(int taskId, int projectId, int selected1 = -1)
        {
            var projectTask = await appContext.ProjectTasks.FirstOrDefaultAsync(pt => pt.TaskId == taskId);
            ViewBag.projectId = projectId;
            ViewBag.statusItems = appContext.Statuses.ToList()
                                  .OrderBy(status => status.StatusName)
                                  .Select(status => new SelectListItem
                                  {
                                      Text = String.Format("{0}", status.StatusName),
                                      Value = status.StatusId.ToString(),
                                      Selected = status.StatusId == selected1
                                  });
            return View(projectTask);
        }
    }
}
