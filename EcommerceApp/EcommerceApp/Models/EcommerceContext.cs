using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EcommerceApp.Models
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext() : base("DefaultConnection")
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

        public System.Data.Entity.DbSet<EcommerceApp.Models.Warehouse> Warehouses { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.Inventory> Inventories { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<EcommerceApp.Models.State> States { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.SaleDetail> SaleDetails { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.OrderDetail> OrderDetails { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.PurchaseDetail> PurchaseDetails { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.OrderDetailTmp> OrderDetailTmps { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.SaleDetailTemp> SaleDetailTemps { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.PurchaseDetailTmp> PurchaseDetailTmps { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.CompanyCustomer> CompanyCustomers { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.Sale> Sales { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.Supplier> Suppliers { get; set; }
        public System.Data.Entity.DbSet<EcommerceApp.Models.Purchase> Purchases { get; set; }

    }
}