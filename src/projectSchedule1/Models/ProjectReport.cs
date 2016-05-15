using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace projectSchedule1.Models
{
    public class ProjectReport
    {
        [Key]
        public int ProjectReportId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime ProjectReportDate { get; set; }
        public string ProjectReportDesc { get; set; }
        public Employee Employee { get; set; }
        public Project Project { get; set; }
        public ICollection<File> Files { get; set; }

    }
}
