using EcommerceApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceApp.Classes
{
    public class CombosHelper:IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public static List<Department> GetDepartments()
        {
            List<Department> departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentId = 0,
                Name = "[Select a department ...]"
            }
                );

            return departments.OrderBy(a => a.Name).ToList();

        }

        public static List<Product> GetProducts(int companyId, bool sw)
        {
            List<Product> products = db.Products.Where(p => p.CompanyId == companyId).ToList();
            return products.OrderBy(p => p.Description).ToList();
        }

        public static List<Product> GetProducts(int companyId)
        {
            List<Product> products = db.Products.Where(p => p.CompanyId == companyId).ToList();
            products.Add(new Product
            {
                ProductId = 0,
                Description = "[Select a product...]",
            });
            return products.OrderBy(p => p.Description).ToList();
        }

        public static List<City> GetCities(int departmentId)
        {
            List<City> cities = db.Cities.Where(c => c.DepartmentId == departmentId).ToList();
            cities.Add(new City
            {
                CityId = 0,
                Name = "[Select a city ...]"
            }
                );

            return cities.OrderBy(a => a.Name).ToList();

        }

        public static List<Company> GetCompanies()
        {
            List<Company> companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyId = 0,
                Name = "[Select a company ...]"
            }
                );

            return companies.OrderBy(a => a.Name).ToList();

        }
        public void Dispose()
        {
            db.Dispose();
        }

        public static List<Customer> GetCustomers(int companyId)
        {
            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerId
                       join co in db.Companies on cc.CompanyId equals co.CompanyId
                       where co.CompanyId == companyId
                       select new { cu }).ToList();

            List<Customer> customers = new List<Customer>();
            foreach(var item in qry)
            {
                customers.Add(item.cu);
            }

            customers.Add(new Customer
            {
                CustomerId = 0,
                FirstName = "[Select a customer...]",
            });
            return customers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        }

        public static List<Supplier> Suppliers(int companyId)
        {
            List<Supplier> suppliers = db.Suppliers.Where(w => w.CompanyId == companyId).ToList();
            suppliers.Add(new Supplier
            {
                SupplierId = 0,
                FirstName = "[Select a supplier...]",
            });
            return suppliers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        }

        public static List<Category> GetCategories(int companyId)
        {
            List<Category> categories = db.Categories.Where(c => c.CompanyId == companyId).ToList();
            categories.Add(new Category
            {
                CategoryId = 0,
                Description = "[Select a category...]",
            });
            return categories.OrderBy(d => d.Description).ToList();

        }

        public static List<Tax> GetTaxes(int companyId)
        {
            List<Tax> taxes = db.Taxes.Where(c => c.CompanyId == companyId).ToList();
            taxes.Add(new Tax
            {
                TaxId = 0,
                Description = "[Select a tax...]",
            });
            return taxes.OrderBy(d => d.Description).ToList();

        }

        public static List<Order> GetOrders(int companyId)
        {
            List<Order> orders = db.Orders.Where(o => o.CompanyId == companyId && o.State.Description == "Создан").ToList();
            return orders.OrderBy(o => o.Date).ToList();
        }

        public static List<Warehouse> GetWarehouses(int companyId)
        {
            List<Warehouse> warehouses = db.Warehouses.Where(w => w.CompanyId == companyId).ToList();
            warehouses.Add(new Warehouse
            {
                WarehouseId = 0,
                Name = "[Select a warehouse...]",
            });
            return warehouses.OrderBy(w => w.Name).ToList();
        }

        public static List<State> GetState()
        {
            List<State> states = db.States.ToList();
            states.Add(new State
            {
                StateId = 0,
                Description = "[Select a state ...]"
            }
                );

            return states.OrderBy(a => a.Description).ToList();
        }
    }
}