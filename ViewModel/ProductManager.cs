using System;
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
        public ObservableSortedSet<Product> Products { get; } = new ObservableSortedSet<Product>();
        public QueryService QueryService { get; } = new QueryService();

        public ProductManager()
        {
            foreach (var product in Product.GetMockProducts())
            {
                QueryService.AddQuery(new Query(Query.QueryType.Add, product));
            }

            Task.Run(() => QueryService.Run());
        }

        public void AddProduct(string name, int number)
        {
            QueryService.AddQuery(new Query(Query.QueryType.Add, new Product(number, name)));
        }

        public void DeleteProduct(string name, int number)
        {
            QueryService.AddQuery(new Query(Query.QueryType.Delete, new Product(number, name)));
        }

        public void UpdateLocalProducts()
        {
            Products.Clear();

            foreach (var product in StockController.GetAllDbProducts().Where(product => product.Number > 0))
            {
                Products.Add(product);
            }
        }

        public void Dispose()
        {
            QueryService.IsActive = false;
            Thread.Sleep(300);
        }
    }
}