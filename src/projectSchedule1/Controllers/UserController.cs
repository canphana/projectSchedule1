using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using projectSchedule1.Models;

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
    }
}
