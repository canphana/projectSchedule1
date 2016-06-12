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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProjectReportDate { get; set; }
        public string ProjectReportDesc { get; set; }
        public Employee Employee { get; set; }
        public Project Project { get; set; }
        public ICollection<File> Files { get; set; }

    }
}
