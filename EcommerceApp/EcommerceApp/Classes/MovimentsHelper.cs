using EcommerceApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EcommerceApp.Classes
{
    public class MovimentsHelper: IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public void Dispose()
        {
            db.Dispose();
        }

         public static Response NewOrder(NewOrderView view, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = new Order
                    {
                        CompanyId = user.CompanyId,
                        CustomerId = view.CustomerId,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateId = DBHelper.GetState("Создан", db),
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();
                    var details = db.OrderDetailTmps.Where(odt => odt.UserName == userName).ToList();

                    foreach (var detail in details)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Description = detail.Description,
                            OrderId = order.OrderId,
                            Price = detail.Price,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate,
                        };

                        db.OrderDetails.Add(orderDetail);
                        db.OrderDetailTmps.Remove(detail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                    throw;
                }
            
            }
        }


        public static Response CancelOrder(int? id, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = db.Orders.Where(o => o.CompanyId == user.CompanyId && o.OrderId == id).FirstOrDefault();
                    var orderDetails = db.OrderDetails.Where(odt => odt.OrderId == order.OrderId).ToList();

                    foreach (var detail in orderDetails)
                    {
                        db.OrderDetails.Remove(detail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                    throw;
                }

            }
        }

        public static Response NewPurchase(NewPurchaseView view, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var details = db.PurchaseDetailTmps.Where(pdt => pdt.UserName == userName).ToList();
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var purchase = new Purchase
                    {
                        CompanyId = user.CompanyId,
                        SupplierId = view.SupplierId,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        WarehouseId = view.WarehouseId,
                        UserId = user.UserId,
                        
                    };
                    db.Purchases.Add(purchase);
                    db.SaveChanges();

                    foreach (var detail in details)
                    {
                        
                        var purchaseDetail = new PurchaseDetail
                        {
                            Description = detail.Description,
                            PurchaseId = purchase.PurchaseId,
                            Cost = detail.Cost,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate,
                        };
                        db.PurchaseDetails.Add(purchaseDetail);                       
                        db.SaveChanges();

                    }

                    foreach (var detail in details)
                    {
                        var duplicate = db.Inventories
                        .Where(i => i.Product.CompanyId == user.CompanyId
                        && i.ProductId == detail.ProductId
                        && i.WarehouseId == purchase.WarehouseId).FirstOrDefault();

                        if (duplicate == null)
                        {
                            var invetary = new Inventory
                            {
                                WarehouseId = purchase.WarehouseId,
                                ProductId = detail.ProductId,
                                Stock = detail.Quantity,
                                Date = DateTime.Now,

                            };
                            db.Inventories.Add(invetary);
                            db.PurchaseDetailTmps.Remove(detail);
                        }
                        else
                        {
                            duplicate.Stock += detail.Quantity;
                            duplicate.Date = DateTime.Now;
                            db.Entry(duplicate).State = EntityState.Modified;
                            db.PurchaseDetailTmps.Remove(detail);
                        }
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                    throw;
                }

            }
        }

        public static Response NewSale(NewSaleView view, string userName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var details = db.SaleDetailTemps.Where(pdt => pdt.UserName == userName).ToList();
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var sale = new Sale
                    {
                        CompanyId = user.CompanyId,
                        UserId = user.UserId,
                        WarehouseId = view.WarehouseId,
                        StateId = DBHelper.GetState("Создан", db),
                        OrderId = details[0].OrderId,
                        Date = DateTime.Now,
                        Remarks = view.Remarks,
                        CustomerId = view.CustomerId,
                    };
                    db.Sales.Add(sale);
                    db.SaveChanges();

                    foreach (var detail in details)
                    {

                        var saleDetail = new SaleDetail
                        {
                            SaleId = sale.SaleId,
                            ProductId = detail.ProductId,
                            Description = detail.Description,
                            Price = detail.Price,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate,
                        };
                        db.SaleDetails.Add(saleDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                    return new Response { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                    throw;
                }

            }
        }

    }
}