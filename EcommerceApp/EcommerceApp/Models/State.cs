﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Required(ErrorMessage ="The field {0} is requerid")]
        [MaxLength(50, ErrorMessage ="The field {0} must be maximun {1} characters length")]
        [Display(Name ="State")]
        [Index("State_Description_Index", IsUnique = true)]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}