using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class Departament
    {
        [Key]
        public int Departamentid { get; set; }
        [Required(ErrorMessage ="The fild {0} is requid")]
        [MaxLength(50, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        public string Name { get; set; }

    }
}