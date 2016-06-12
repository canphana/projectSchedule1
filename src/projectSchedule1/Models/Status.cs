using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace projectSchedule1.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        [Display(Name="Status")]
        public string StatusName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string StatusDesc { get; set; }
        public ProjectTask ProjectTask{get; set;}
        public Project Project { get; set; }
        public Employee Employee { get; set; }

    }
}
