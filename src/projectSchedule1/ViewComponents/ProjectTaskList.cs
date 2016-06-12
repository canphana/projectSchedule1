using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using projectSchedule1.Models;
using Microsoft.AspNet.Mvc.Rendering;
using Sakura.AspNet;
using Microsoft.AspNet.Http;

namespace projectSchedule1.ViewComponents
{
    public class ProjectTaskList: ViewComponent
    {
        private readonly AppDbContext ctx;

        public ProjectTaskList(AppDbContext c)
        {
            ctx = c;
        }

        public IViewComponentResult Invoke(int projectId)
        {
            try
            {
                var projectTaskList = from t
                                      in ctx.ProjectTasks
                                      select t;
                int? page = 1;
                if (HttpContext.Session.GetString("UserType").Equals("Admin"))
                {
                    projectTaskList = from t
                                        in ctx.ProjectTasks.Include(t => t.Project).Include(t => t.Status)
                                      where t.ProjectId == projectId
                                      select t;
                }
                else
                {
                    int employeeId = (int)HttpContext.Session.GetInt32("UserId");
                    projectTaskList = from t
                                        in ctx.ProjectTasks.Include(t => t.Project).Include(t => t.Status)
                                     where (t.ProjectId == projectId && t.EmployeeTasks.Count(et => et.EmployeeId == employeeId)>=1)
                                      select t;
                }
                    if (projectTaskList != null)
                    {
                        var pageSize = 5;
                        var pageNumber = page ?? 1;
                        return View(projectTaskList.ToPagedList(pageSize, pageNumber));
                    }
                
            }
            catch (Exception)
            {
                return View();
            }
            return View();
        }
    }
}
