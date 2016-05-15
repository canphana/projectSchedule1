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
        [Column(TypeName = "nvarchar(200)")]
        public string EmployeeFirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string EmployeeLastName { get; set; }

        [Required]
        public DateTime EmployeeBirthDate { get; set; }

        public string EmployeeAddr { get; set; }
        public string EmployeePhoneNo { get; set; }
        public string EmployeeEmail { get; set; }
        public int StatusId { get; set; }

        public Status Status { get; set; }
        public ICollection<EmployeeTask> EmployeeTasks { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
