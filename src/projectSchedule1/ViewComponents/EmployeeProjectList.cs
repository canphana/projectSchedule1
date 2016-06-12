using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using projectSchedule1.Models;
using Microsoft.AspNet.Mvc.Rendering;
using Sakura.AspNet;

namespace projectSchedule1.ViewComponents
{
    public class EmployeeProjectList: ViewComponent
    {
        private readonly AppDbContext ctx;

        public EmployeeProjectList(AppDbContext c)
        {
            ctx = c;
        }


        public IViewComponentResult Invoke(int projectId)
        {
            int? page=1;
            try
            {
                var emProjectList = ctx.Employees
                                    .Where(e => e.EmployeeProjects.Any(ep => ep.ProjectId == projectId))
                                    .Include(e=>e.Status);
                if (emProjectList != null)
                {
                    var pageSize = 5;
                    var pageNumber = (page ?? 1);
                    return View(emProjectList.ToPagedList(pageSize, pageNumber));
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
