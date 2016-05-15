using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        [Column(TypeName ="nvarchar(200)")]
        public string TaskDesc { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime TaskEndDate { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ProjectId { get; set; }
       
        public Project Project { get; set; }

        public ICollection<EmployeeTask> EmployeeTasks { get; set; }
    }
}
