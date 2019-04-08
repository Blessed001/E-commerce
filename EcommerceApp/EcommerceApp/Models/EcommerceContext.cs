using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class EcommerceContext:DbContext 
    {
        public EcommerceContext():base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<EcommerceApp.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Tax> Taxes { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Product> Products { get; set; }
    }
}