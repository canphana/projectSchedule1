using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using projectSchedule1.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace projectSchedule1.Controllers
{
    public class FileController : Controller
    {
        // GET: /<controller>/
        [FromServices]
        public AppDbContext appContext { get; set; }
        public ActionResult Index(int id)
        {
            var fileToRetrieve = appContext.Files.SingleOrDefault(f=>f.FileId==id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}
