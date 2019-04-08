using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(50, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [Display(Name = "City")]
        [Index("City_DepartmentId_Name_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [Range(1,double.MaxValue,ErrorMessage ="You mast select a {0}")]
        [Index("City_DepartmentId_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<User> Users { get; set; }


    }
}