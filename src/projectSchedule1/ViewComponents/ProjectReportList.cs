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
    public class ProjectReportList : ViewComponent
    {
        private readonly AppDbContext ctx;

        public ProjectReportList(AppDbContext c)
        {
            ctx = c;
        }


        public IViewComponentResult Invoke(int projectId)
        {
            int? page = 1;
            var reportList = ctx.ProjectReports.Where(pr => pr.ProjectId==projectId);
            if (reportList != null)
            {
                var pageSize = 5;
                var pageNumber = (page ?? 1);
                return View(reportList.ToPagedList(pageSize, pageNumber));
            }
            return View();
        }
    }
}
