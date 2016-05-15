using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string ProjectName { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string ProjectDesc { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
       
        public ICollection<Task> Tasks { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<ProjectReport> ProjectReports { get; set; }
    }
}
