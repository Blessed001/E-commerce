using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace EcommerceApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(256, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [Display(Name = "E-mail")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(50, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(50, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(20, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }   

        [Required(ErrorMessage = "The fild {0} is requid")]
        [MaxLength(100, ErrorMessage = "The field {0} mast be at maximum {1} characters lenght")]
        public string Adress {get; set;}

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The fild {0} is requid")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "Employee")]
        public string FullName { get { return string.Format("{0} {1}", FirstName,LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        public virtual Department Department { get; set; }
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }


    }
}