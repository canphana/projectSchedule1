using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class ProjectTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(200)")]
        [Display(Name = "Task Description")]
        public string TaskDesc { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Task StartDate")]
        public DateTime TaskStartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Task EndDate")]
        public DateTime TaskEndDate { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ProjectId { get; set; }
       
        public Project Project { get; set; }

        public ICollection<EmployeeTask> EmployeeTasks { get; set; }
    }
}
