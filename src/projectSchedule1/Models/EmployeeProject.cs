using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class EmployeeProject
    {
        
        public int EmployeeId { get; set; }
     
        public int ProjectId { get; set; }

        public Project Project {get; set;}
        public Employee Employee { get; set; }
    }
}
