using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Index("Company_CompanyId_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(50, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        [Display(Name = "Warehouse")]
        [Index("Company_CompanyId_Name_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(20, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(100, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        public string Address { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }


    }
}