using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace projectSchedule1.ViewModels
{
    public class LoginModels
    {
        [Required]
        public string userName { get; set;}
        [Required]
        public string passWord { get; set;}
    }
}
