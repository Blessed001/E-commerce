using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class EcommerceContext:DbContext 
    {
        public EcommerceContext():base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Departament> Departaments { get; set; }
    }
}