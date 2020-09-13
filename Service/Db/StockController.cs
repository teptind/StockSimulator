using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductsCounting.Infrastructure;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;

namespace ProductsCounting.Service.Db
{
    public class StockController
    {
        private const int TriesNum = 5;
        private static void CheckProductNumber(Product product, int delta)
        {
            if (delta > 0)
            {
                if (product.Number > Constants.MaxAmount - delta)
                {
                    throw new ManagerException($"The overall number of {product.Name} is too big");
                }
            }
            else
            {
                if (product.Number + delta < 0)
                {
                    throw new ManagerException($"The number of {product.Name} in stock is not enough");
                }
            }
        }

        public static void AddProduct(Product product)
        {
            for (int tryIndex = 0; tryIndex < TriesNum; ++tryIndex)
            {
                try
                {
                    using (var db = new StockDbContext())
                    {
                        var entity = db.Stock.FirstOrDefault(item => item.Name.Equals(product.Name));
                        if (entity != null)
                        {
                            CheckProductNumber(entity, product.Number);
                            entity.Number += product.Number;
                        }
                        else
                        {
                            db.Add(new Product(product.Number, product.Name));
                        }

                        db.SaveChanges();
                    }

                    return;
                }
                catch (Exception e)
                {
                    if (!(e is DbUpdateConcurrencyException))
                        throw;
                }
            }
        }

        public static void DeleteProduct(Product product)
        {
            for (int tryIndex = 0; tryIndex < TriesNum; ++tryIndex)
            {
                try
                {
                    using (var db = new StockDbContext())
                    {
                        var entity = db.Stock.FirstOrDefault(item => item.Name.Equals(product.Name));
                        if (entity == null)
                            throw new ManagerException($"There is no {product.Name} in DB");

                        CheckProductNumber(entity, -product.Number);
                        entity.Number -= product.Number;
                        db.SaveChanges();
                    }

                    return;
                }
                catch (Exception e)
                {
                    if (!(e is DbUpdateConcurrencyException))
                        throw;
                }
            }
        }

        public static List<Product> GetAllDbProducts()
        {
            using (var db = new StockDbContext())
            {
                return db.Stock.ToList().Select(
                    productEntity =>
                        new Product(productEntity.Number, productEntity.Name)).ToList();
            }
        }
    }
}