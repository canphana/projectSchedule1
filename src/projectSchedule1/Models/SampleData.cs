using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Data.Entity;

namespace projectSchedule1.Models
{
    public class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<AppDbContext>();
            context.Database.Migrate();
            if(!context.Employees.Any())
            {
                var active = context.Statuses.Add(
                    new Status {StatusName = "Active", StatusDesc="Active status" }).Entity;
                var inActive = context.Statuses.Add(
                   new Status { StatusName = "Inactive", StatusDesc = "Inactive status" }).Entity;
                var completed = context.Statuses.Add(
                   new Status { StatusName = "Completed", StatusDesc = "Completed status" }).Entity;

               var admin= context.AccountTypes.Add(new AccountType { AccountTypeName = "Admin", AccountTypeDesc = "Admin account" }).Entity;
               var user=  context.AccountTypes.Add(new AccountType { AccountTypeName = "User", AccountTypeDesc = "User account" }).Entity;

                var emAdmin = context.Employees.Add(
                    new Employee { EmployeeFirstName = "Phan", EmployeeLastName = "Trương", EmployeePhoneNo = "01686511428", EmployeeBirthDate = Convert.ToDateTime("09/07/1993"), EmployeeAddr="Hanoi, VietNam", EmployeeEmail="tbnp9793@gmail.com", Status = active }).Entity;

                context.LoginAcounts.Add(new LoginAccount { LoginAccountName = "admin", LoginAccountPassword = "admin", Employee=emAdmin, AccountType= admin});
            }
        }
    }
}
