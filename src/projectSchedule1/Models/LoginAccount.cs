using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class LoginAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginAccountId { get; set; }
        [Required]
        [Display(Name="Account Name")]
        public string LoginAccountName { get; set; }
        [Required]
        [Display(Name = "Account Password")]
        public string LoginAccountPassword { get; set; }
        [Required]
        [Display(Name = "Account Type")]
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
        [Required]
        [Display(Name = "Account Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
