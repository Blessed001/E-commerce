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
    [Authorize(Roles ="User")]
    public class PurchasesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        public ActionResult Index()
        {
            User user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var purchases = db.Purchases.Where(p => p.CompanyId == user.CompanyId).Include(p => p.Supplier).Include(p => p.Warehouse);
            return View(purchases.ToList());
        }

        public ActionResult AddProduct()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId, true), "ProductId", "Description");
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddProduct(AddPurchaseView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var purchaseDetailTmp = db.PurchaseDetailTmps.Where(pdt => pdt.UserName == User.Identity.Name && pdt.ProductId == view.ProductId).FirstOrDefault();
                if (purchaseDetailTmp == null)
                {
                    var product = db.Products.Find(view.ProductId);
                    purchaseDetailTmp = new PurchaseDetailTmp
                    {
                        Description = product.Description,
                        Cost = view.Cost,
                        ProductId = product.ProductId,
                        Quantity = view.Quantity,
                        TaxRate = product.Tax.Rate,
                        UserName = User.Identity.Name,                        
                    };

                    db.PurchaseDetailTmps.Add(purchaseDetailTmp);
                }
                else
                {
                    purchaseDetailTmp.Quantity += view.Quantity;
                    db.Entry(purchaseDetailTmp).State = EntityState.Modified;
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
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var purchaseDetailTmps = db.PurchaseDetailTmps.Where(odt => odt.UserName == User.Identity.Name && odt.ProductId == id).FirstOrDefault();
            if (purchaseDetailTmps == null)
            {
                return HttpNotFound();
            }
            db.PurchaseDetailTmps.Remove(purchaseDetailTmps);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Create");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(purchaseDetailTmps);
        }

        public ActionResult CleanPurchaseDetails()
        {
            var purchaseDetailTmps = db.PurchaseDetailTmps.Where(pdt => pdt.UserName == User.Identity.Name).ToList();
            if (purchaseDetailTmps == null)
            {
                return HttpNotFound();
            }
            foreach (var detail in purchaseDetailTmps)
            {
                db.PurchaseDetailTmps.Remove(detail);
            }
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Create");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return RedirectToAction("Create");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            var view = new PurchaseDetailsView
            {
                Details = db.PurchaseDetails.Where(odt => odt.PurchaseId == id).ToList()
            };
            if (purchase == null)
            {
                return HttpNotFound();
            }

            return PartialView(view);
        }

        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.SupplierId = new SelectList(CombosHelper.Suppliers(user.CompanyId), "SupplierId", "FullName");
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(user.CompanyId), "WarehouseId", "Name");

            var view = new NewPurchaseView
            {
                Date = DateTime.Now,
                Details = db.PurchaseDetailTmps.Where(pdt => pdt.UserName == User.Identity.Name).OrderByDescending(pdt => pdt.PurchaseDetailTmpId).ToList(),

            };
            return View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewPurchaseView view)
        {
            if (ModelState.IsValid)
            {
                var responseMoviment = MovimentsHelper.NewPurchase(view, User.Identity.Name);
                if (responseMoviment.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, responseMoviment.Message);
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.SupplierId = new SelectList(CombosHelper.Suppliers(user.CompanyId), "SupplierId", "FullName");
            ViewBag.WarehouseId = new SelectList(CombosHelper.GetWarehouses(user.CompanyId), "WarehouseId", "Name");
            view.Details = db.PurchaseDetailTmps.Where(pdt => pdt.UserName == User.Identity.Name).ToList();
            return View(view);
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
