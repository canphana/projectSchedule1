using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectSchedule1.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        [Required]
        [Display(Name ="Client Name")]
        public string ClientName { get; set; }
        [Required]
        [Display(Name = "Client Address")]
        public string ClientAddress { get; set; }
        [Required]
        [Display(Name = "Client Email")]
        public string ClientEmail { get; set; }
        [Required]

        [Display(Name = "Client Phone Number")]
        public string ClientPhoneNo { get; set; }


    }
}
