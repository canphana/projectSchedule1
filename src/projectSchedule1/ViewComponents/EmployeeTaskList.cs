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
    public class EmployeeTaskList : ViewComponent
    {
        private readonly AppDbContext ctx;

        public EmployeeTaskList(AppDbContext c)
        {
            ctx = c;
        }


        public IViewComponentResult Invoke(int TaskId)
        {
            int? page = 1;
            try
            {
                var emTaskList = ctx.Employees
                                    .Where(e => e.EmployeeTasks.Any(ep => ep.TaskId == TaskId))
                                    .Include(e => e.Status);
                if (emTaskList != null)
                {
                    var pageSize = 5;
                    var pageNumber = (page ?? 1);
                    return View(emTaskList.ToPagedList(pageSize, pageNumber));
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
