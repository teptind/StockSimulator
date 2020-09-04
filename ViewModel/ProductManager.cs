using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;

namespace ProductsCounting.ViewModel
{
    public class ProductManager
    {
        public ObservableSortedSet<Product> Products { get; }
        public Dictionary<string, Product> ProductNames { get; }

        public ProductManager()
        {
            Products = new ObservableSortedSet<Product>();
            ProductNames = new Dictionary<string, Product>();
            foreach (var product in Product.GetMockProducts())
            {
                Products.Add(product);
                ProductNames.Add(product.Name, product);
            }
        }

        public void AddProduct(string name, int number)
        {
            if (!ProductNames.ContainsKey(name))
            {
                var newProduct = new Product(number, name);
                Products.Add(newProduct);
                ProductNames[name] = newProduct;
            }
            else
            {
                if (int.MaxValue - number < ProductNames[name].Number)
                {
                    throw new ManagerException($"The total amount of {name} is too big");
                }

                ProductNames[name].Number += number;
            }
        }

        public void DeleteProduct(string name, int number)
        {
            if (!ProductNames.ContainsKey(name))
            {
                throw new ManagerException($"There is no product with name {name}");
            }

            if (ProductNames[name].Number >= number)
            {
                ProductNames[name].Number -= number;
                if (ProductNames[name].Number == 0)
                {
                    Products.Remove(ProductNames[name]);
                    ProductNames.Remove(name);
                }
            }
            else
            {
                throw new ManagerException($"There is not enough {name} in stock");
            }
        }
    }
}