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
        [Required]
        [Display(Name ="Project Name")]
        [Column(TypeName = "nvarchar(200)")]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Project Description")]
        [Column(TypeName = "nvarchar(200)")]
        public string ProjectDesc { get; set; }
        [Required]
        [Display(Name = "Project Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProjectStartDate { get; set; }
        [Required]
        [Display(Name = "Project End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProjectEndDate { get; set; }
        [Display(Name = "Project Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
       
        public ICollection<ProjectTask> ProjectTasks { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<ProjectReport> ProjectReports { get; set; }
    }
}
