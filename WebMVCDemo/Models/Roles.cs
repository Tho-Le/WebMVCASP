using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVCDemo.Models
{
    public class Roles
    {
        [Required]
        public string RoleName { get; set; }
    }
}
