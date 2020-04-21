using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVCDemo.Models
{
    public class Country
    {
        public int Id { get; set; }
        //The required attribute is client side validation. It with not allow the field to be empty when submitting data
        [Required]
        public string name { get; set; }
        public int population { get; set; }
        public int density { get; set; }
        public int sqaureMiles { get; set; }



    }
}
