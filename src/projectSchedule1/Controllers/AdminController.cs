using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using projectSchedule1.Models;
using projectSchedule1.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;

namespace projectSchedule1.Controllers
{
    public class AdminController: Controller
    {

        [FromServices]
        public AppDbContext appContext { get; set; }

        public IActionResult UserDetails()
        {

           int id = (int)HttpContext.Session.GetInt32("UserId");
            Employee employee = appContext.Employees.SingleOrDefault(e=>e.EmployeeId==id); 
            return View(employee);
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
    
        public IActionResult Employee()
        {
            var employeeList = appContext.Employees
                .Include(e => e.Status)
                .OrderBy(e => e.EmployeeLastName)
                .ToList();
                return View(employeeList);
        }

    }
}
