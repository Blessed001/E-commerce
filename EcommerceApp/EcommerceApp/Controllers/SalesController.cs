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

namespace EcommerceApp.Controllers
{
    public class SalesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();




        [HttpPost]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var saleDetailTmp = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductId == view.ProductId).FirstOrDefault();
                if (saleDetailTmp == null)
                {
                    var product = db.Products.Find(view.ProductId);
                    saleDetailTmp = new SaleDetailTemp
                    {
                        Description = product.Description,
                        Price = product.Price,
                        ProductId = product.ProductId,
                        Quantity = view.Quantity,
                        TaxRate = product.Tax.Rate,
                        UserName = User.Identity.Name,
                    };

                    db.SaleDetailTemps.Add(saleDetailTmp);
                }
                else
                {
                    saleDetailTmp.Quantity += view.Quantity;
                    db.Entry(saleDetailTmp).State = EntityState.Modified;
                }
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Create");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId), "ProductId", "Description");
            return PartialView(view);
        }
        public ActionResult AddProduct()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId, true), "ProductId", "Description");
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddProductFromOrders(AddOrderView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var saleDetailTmp = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name).FirstOrDefault();
                var order = db.Orders.Find(view.OrderId);
                var orderDetail = db.OrderDetails.Where(odt => odt.Order.CompanyId == user.CompanyId && odt.Order.OrderId == order.OrderId).ToList();

                foreach (var detail in orderDetail)
                {
                    saleDetailTmp = new SaleDetailTemp
                    {
                        Description = detail.Description,
                        Price = detail.Price,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        TaxRate = detail.TaxRate,
                        UserName = User.Identity.Name,
                        OrderId = view.OrderId,
                    };

                    db.SaleDetailTemps.Add(saleDetailTmp); 
                }
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Create");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            ViewBag.OrderId = new SelectList(CombosHelper.GetOrders(user.CompanyId), "OrderId", "OrderId");
            return PartialView(view);
        }
        public ActionResult AddProductFromOrders()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.OrderId = new SelectList(CombosHelper.GetOrders(user.CompanyId), "OrderId", "OrderId");
            return PartialView();
        }

        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var SaleDetailTmp = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductId == id).FirstOrDefault();
            if (SaleDetailTmp == null)
            {
                return HttpNotFound();
            }
            db.SaleDetailTemps.Remove(SaleDetailTmp);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Create");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(SaleDetailTmp);
        }

        public ActionResult CleanOrderDetails()
        {
            var SaleDetailTmp = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name).ToList();
            if (SaleDetailTmp == null)
            {
                return HttpNotFound();
            }
            foreach (var detail in SaleDetailTmp)
            {
                db.SaleDetailTemps.Remove(detail);
            }
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Create");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return RedirectToAction("Create");
        }
        // GET: Sales
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var sales = db.Sales.Where(s => s.CompanyId == user.CompanyId).Include(s => s.Company).Include(s => s.State).Include(s => s.User).Include(s => s.Warehouse);
            return View(sales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(user.CompanyId), "WarehouseId", "Name");
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName");

            var view = new NewSaleView
            {
                Date = DateTime.Now,
                Details = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name).OrderByDescending(odt => odt.SaleDetailTempId).ToList(),
            };
            return View(view);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewSaleView view)
        {
         
            if (ModelState.IsValid)
            {
                var responseMoviment = MovimentsHelper.NewSale(view, User.Identity.Name);
                if (responseMoviment.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, responseMoviment.Message);
            }
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(user.CompanyId), "WarehouseId", "Name", view.WarehouseId);
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName", view.CustomerId);
            view.Details = db.SaleDetailTemps.Where(odt => odt.UserName == User.Identity.Name).ToList();
            return View(view);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(sale.CompanyId), "WarehouseId", "Name", sale.WarehouseId);
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(sale.CompanyId), "CustomerId", "FullName", sale.CustomerId);

            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(sale.CompanyId), "WarehouseId", "Name", sale.WarehouseId);
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(sale.CompanyId), "CustomerId", "FullName", sale.CustomerId);

            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
            db.SaveChanges();
            return RedirectToAction("Index");
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
