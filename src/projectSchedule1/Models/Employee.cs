using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required]
        [Display(Name = "Employee First Name")]
        [Column(TypeName = "nvarchar(200)")]     
        public string EmployeeFirstName { get; set; }
        [Required]
        [Display(Name = "Employee Last Name")]
        [Column(TypeName = "nvarchar(200)")]
        public string EmployeeLastName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Employee Birth Date")]
        public DateTime EmployeeBirthDate { get; set; }
        [Required]
        [Display(Name = "Employee Address")]
        public string EmployeeAddr { get; set; }
        [Required]
        [Display(Name = "Employee Phone Number")]
        public string EmployeePhoneNo { get; set; }
        [Required]
        [Display(Name = "Employee Email")]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Email address must be a valid email address.")]
        public string EmployeeEmail { get; set; }
        [Display(Name = "Employee Status")]
        public int StatusId { get; set; }

        public Status Status { get; set; }
        public ICollection<EmployeeTask> EmployeeTasks { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
