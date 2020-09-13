using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProductsCounting.Infrastructure.DataStructures;
using ProductsCounting.Model;
using ProductsCounting.Service.Db;

namespace ProductsCounting.ViewModel
{
    public class ProductManager : IDisposable
    {
        public SortedSet<Product> Products { get; } = new SortedSet<Product>();
        public QueryService QueryService { get; } = new QueryService();

        public Action<Product> OnProductAdd;
        public Action OnStockClear;
        public Action OnQuerySent;
        public Action<Query> OnQueryAdd;
        public Action<string> OnQueryDbFail;

        public ProductManager()
        {
            QueryService.OnQuerySent += () => { OnQuerySent?.Invoke(); };
            QueryService.OnOperationDeny += msg => { OnQueryDbFail?.Invoke(msg); };
        }

        public void StartWork()
        {
            foreach (var product in Product.GetMockProducts())
            {
                AddProduct(product.Name, product.Number);
            }

            Task.Run(() => QueryService.Run());
        }

        public void AddProduct(string name, int number)
        {
            var query = new Query(Query.QueryType.Add, new Product(number, name));
            QueryService.AddQuery(query);
            OnQueryAdd?.Invoke(query);
        }

        public void DeleteProduct(string name, int number)
        {
            var query = new Query(Query.QueryType.Delete, new Product(number, name));
            QueryService.AddQuery(query);
            OnQueryAdd?.Invoke(query);
        }

        public void UpdateLocalProducts()
        {
            Products.Clear();
            OnStockClear?.Invoke();

            foreach (var product in StockController.GetAllDbProducts().Where(product => product.Number > 0))
            {
                Products.Add(product);
                OnProductAdd?.Invoke(product);
            }
        }

        public void Dispose()
        {
            QueryService.IsActive = false;
            Thread.Sleep(300);
        }
    }
}