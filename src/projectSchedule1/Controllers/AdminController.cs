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
using Microsoft.AspNet.Hosting;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace projectSchedule1.Controllers
{
    public class AdminController: Controller
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

           int id = (int)HttpContext.Session.GetInt32("UserId");
            Employee employee = appContext.Employees.Include(e=>e.Files).SingleOrDefault(e=>e.EmployeeId==id);
             
            return View(employee);
        }

        public async Task<ActionResult> EditDetails(int id, int selected=-1)
        {
            Employee employee = await appContext.Employees.Include(e=>e.Files).SingleOrDefaultAsync(e=>e.EmployeeId==id);
           
            if (employee == null)
            {
                return RedirectToAction("AdminDetails");
               
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(int id, [Bind("EmployeeFirstName", "EmployeeLastName", "EmployeeAddr", "EmployeeEmail", "EmployeePhoneNo", "StatusId","EmployeeBirthDate")]Employee employee, IFormFile upload)
        {
            try
            {
                employee.EmployeeId = id;
                if (upload != null && upload.Length > 0)
                {
                    if (employee.Files!=null)
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
                appContext.Employees.Attach(employee);
                appContext.Entry(employee).State = EntityState.Modified;
                await appContext.SaveChangesAsync();
                return RedirectToAction("AdminDetails");

            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Unable to save change.");
            }
            return View();
        } 

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModels cpm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var account = await appContext.LoginAcounts
                        .SingleOrDefaultAsync(la=>la.LoginAccountPassword==cpm.OldPassword);
                    if(account!=null)
                    {
                        account.LoginAccountPassword = cpm.ConfirmNewPassword;
                        appContext.LoginAcounts.Attach(account);
                        await appContext.SaveChangesAsync();
                        return RedirectToAction("AdminDetails");
                    }
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "There is a problem to change your password");
            }
            return View(cpm);
        }

        public IActionResult CreateProject(int selected1 = -1, int selected2 = -1)
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
        public IActionResult Project()
        {
            var projectList = appContext.Projects
                .Include(p => p.Client).Include(p=>p.Status).ToList();
            if (projectList != null)
            {
                return View(projectList);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "There is no project yet.");
                return View();
            }
        }

        public IActionResult CreateEmployee(int selected=-1)
        {
            ViewBag.Items = appContext.Statuses.ToList()
                .OrderBy(status => status.StatusName)
                 .Select(status => new SelectListItem
        {
            Text = String.Format("{0}", status.StatusName),
            Value =status.StatusId.ToString(),
            Selected = status.StatusId == selected
        }); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee([Bind("EmployeeFirstName", "EmployeeLastName", "EmployeeEmail", "EmployeeAddr", "EmployeeBirthDate","EmployeePhoneNo","StatusId")] Employee employee, IFormFile upload)
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
        public IActionResult Employee()
        {
            var employeeList = appContext.Employees
                .Include(e => e.Status)
                .OrderBy(e => e.EmployeeLastName)
                .ToList();
            if (employeeList != null)
            {
                return View(employeeList);
            }
            else
            {
               
                return View();
            }
        }

        public IActionResult LoginAccount()
        {
            var accountList = new List<AccountModels>();
            try
            {
                var loginAccountList = appContext.LoginAcounts
                    .Include(act=>act.AccountType)
                    .Include(e=>e.Employee)
                    .ToList();
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

                return View(accountList);
            }
            catch
            {
                return View();
            }

        }

    }
}
