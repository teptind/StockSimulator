using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;
using ProductsCounting.Service.Db.Entities;

namespace ProductsCounting.Service.Db
{
    public class StockController
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void AddProduct(Product product)
        {
            using (var db = new StockDbContext())
            {
                try
                {
                    var entity = db.Stock.FirstOrDefault(item => item.NameId.Equals(product.Name));
                    if (entity != null)
                    {
                        entity.Number += product.Number;
                    }
                    else
                    {
                        db.Add(new ProductEntity(product.Name, product.Number));
                    }

                    db.SaveChanges();
                }
                catch (DbException e)
                {
                    throw new ManagerException($"Cannot update DB: {e.Message}");
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void DeleteProduct(Product product)
        {
            using (var db = new StockDbContext())
            {
                try
                {
                    var entity = db.Stock.FirstOrDefault(item => item.NameId.Equals(product.Name));
                    if (entity == null)
                        throw new ManagerException($"There is no {product.Name} in DB");

                    entity.Number -= product.Number;
                    db.Add(new ProductEntity(product.Name, product.Number));
                    db.SaveChanges();
                }
                catch (Exception e) when (e is DbException || e is InvalidOperationException)
                {
                    throw new ManagerException($"Cannot update DB: {e.Message}");
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static List<Product> GetAllDbProducts()
        {
            using (var db = new StockDbContext())
            {
                try
                {
                    return db.Stock.ToList().Select(
                        productEntity => 
                            new Product(productEntity.Number, productEntity.NameId)).ToList();
                } 
                catch (DbException e)
                {
                    throw new ManagerException($"Something went wrong while copying DB: {e.Message}");
                }
            }
        }
    }
}