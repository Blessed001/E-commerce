using EcommerceApp.Models;
using System;
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
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentId = 0,
                Name = "[Select a department ...]"
            }
                );

            return departments.OrderBy(a => a.Name).ToList();

        }

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();
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
            var companies = db.Companies.ToList();
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
    }
}