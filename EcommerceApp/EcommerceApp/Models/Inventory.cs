using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd-HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Update")]
        public DateTime Date { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Display(Name = "In Stock")]
        public double Stock { get; set; }


        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }

    }
}