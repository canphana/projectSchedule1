using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace projectSchedule1.ViewModels
{
    public class ChangePasswordModels
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare("NewPassword",ErrorMessage ="Password do not macth")]
        public string ConfirmNewPassword { get; set; }
    }
}
