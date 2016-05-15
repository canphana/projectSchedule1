using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class AccountType
    {
        [Key]
        public int AccountTypeId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string AccountTypeName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string AccountTypeDesc { get; set; }
    }
}
