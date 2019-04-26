using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }


        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Index("Product_CompanyId_Description_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(50, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        [Index("Product_CompanyId_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Product")]
        public string Description { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [MaxLength(13, ErrorMessage = " The field {0} mast be maximum {1} character length")]
        [Index("Product_CompanyId_BarCode_Index", 2, IsUnique = true)]
        [Display(Name = "Bar Cade")]
        public string BarCode { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = " The field {0} is requerid! ")]
        [Range(1, double.MaxValue, ErrorMessage = "You mast select a {0}")]
        [Display(Name = "Tax")]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "The field {0} is requerid!")]
        [Range(0, double.MaxValue, ErrorMessage = "You mast select a {0} between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Display(Name = "In Stock")]
        public double Stock { get { return Inventories == null ? 0 : Inventories.Sum(i => i.Stock); } }

        public virtual Company Company { get; set; }
        public virtual Category Category { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<OrderDetailTmp> OrderDetailTmps { get; set; }
        public virtual ICollection<PurchaseDetailTmp> PurchaseDetailsTmps { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        public virtual ICollection<SaleDetailTemp> SaleDetailTemps { get; set; }







    }
}