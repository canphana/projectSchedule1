using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace projectSchedule1.ViewModels
{
    public class AccountModels
    {
        public int AccountId { get; set; }
        [Display(Name ="Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
        [Display(Name = "Employee")]
        public string AccountEmployee { get; set; }
    }
}
