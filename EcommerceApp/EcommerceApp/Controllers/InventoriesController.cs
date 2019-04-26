using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcommerceApp.Classes;
using EcommerceApp.Models;
using PagedList;

namespace EcommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class InventoriesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

    
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            var inventories = db.Inventories
                .Include(i => i.Product).Include(i => i.Warehouse)
                .Where(i => i.Product.CompanyId  == user.CompanyId)
                .OrderBy(i => i.Product.Description);
            return View(inventories.ToPagedList((int)page, 10));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
