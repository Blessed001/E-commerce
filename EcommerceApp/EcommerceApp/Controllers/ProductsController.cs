using EcommerceApp.Classes;
using EcommerceApp.Models;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace EcommerceApp.Controllers
{
    [Authorize(Roles = "User")]
    public class ProductsController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        public ActionResult Index(int? page = null)
        {
           
            var user = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();
            page = (page ?? 1);
            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Tax)
                .Where(p => p.CompanyId == user.CompanyId).OrderBy(c => c.ProductId).ThenBy(c => c.ProductId);
            return View(products.ToPagedList((int)page, 8));
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);


        }

        public ActionResult Create()
        {
            var user = db.Users
              .Where(u => u.UserName == User.Identity.Name)
              .FirstOrDefault();
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Description");
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Description");
            var product = new Product { CompanyId = user.CompanyId, };
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var user = db.Users
             .Where(u => u.UserName == User.Identity.Name)
             .FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    if (product.ImageFile != null)
                    {
                        var folder = "~/Content/ProductsImage";
                        var file = string.Format("{0}{1}.jpg", product.ProductId, product.CompanyId);
                        var responseImage = FilesHelper.UploadPhoto(product.ImageFile, folder, file);
                        if (responseImage)
                        {

                            var pic = string.Format("{0}/{1}", folder, file);
                            product.Image = pic;
                            db.Entry(product).State = EntityState.Modified;
                            response = DBHelper.SaveChanges(db);
                            if (response.Succeeded)
                            {
                                return RedirectToAction("Index");
                            }
                            
                        }
                    }
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
                
            }

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ProductsImage";
                    var file = string.Format("{0}{1}.jpg", product.ProductId,product.CompanyId);
                    var responseImage = FilesHelper.UploadPhoto(product.ImageFile, folder, file);
                    if (responseImage)
                    {

                        pic = string.Format("{0}/{1}", folder, file);
                        product.Image = pic;

                    }

                }
                db.Entry(product).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(product); 
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
