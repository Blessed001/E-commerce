using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class City
    {
        [Key]
        public int Cityid { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(50, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [Display(Name = "City")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        public int Departamentid { get; set; }

        public virtual Departament Departament { get; set; }

    }
}