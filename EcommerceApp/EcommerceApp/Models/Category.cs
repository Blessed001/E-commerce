using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(50, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        [Index("Category_CompanyId_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Category")]
        public string Description { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Index("Category_CompanyId_Description_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}