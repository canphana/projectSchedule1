using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using projectSchedule1.Models;
using projectSchedule1.ViewModels;
using Microsoft.AspNet.Http;

namespace projectSchedule1.Controllers
{
    public class LoginController: Controller
    {
        [FromServices]
        public  AppDbContext appContext { get; set; }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModels lm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var account = appContext.LoginAcounts.FirstOrDefault(la => la.LoginAccountName == lm.userName);
                    if(lm.passWord==account.LoginAccountPassword)
                    {
                        var accountType = appContext.AccountTypes.FirstOrDefault(act => act.AccountTypeId == account.AccountTypeId);
                        HttpContext.Session.SetInt32("UserId", account.EmployeeId);
                        HttpContext.Session.SetString("UserType", accountType.AccountTypeName.ToString());
                        if (HttpContext.Session.GetString("UserType").Equals("Admin"))
                        {
                            return RedirectToAction("UserDetails", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The account or password you type may not been correct.");
                    }
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError(string.Empty, "The account or password you type may not been correct.");
            }
            return View();
        }
    }
}
