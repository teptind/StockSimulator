using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using ProductsCounting.Infrastructure.DataStructures;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;
using ProductsCounting.Service;

namespace ProductsCounting.ViewModel
{
    public class MainWindowViewModel
    {
        public readonly ProductManager ProductManager = new ProductManager();
        public ObservableQueue<Query> QueryQueue { get; } = new ObservableQueue<Query>();
        public ObservableSortedSet<Product> ProductSet { get; } = new ObservableSortedSet<Product>();
        private Dictionary<string, Product> _productNames = new Dictionary<string, Product>();

        public MainWindowViewModel()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            ProductManager.OnStockClear += () =>
            {
                ProductSet.Clear();
                _productNames.Clear();
            };

            ProductManager.OnProductAdd += product =>
            {
                if (_productNames.ContainsKey(product.Name))
                {
                    _productNames[product.Name].Number += product.Number;
                }
                else
                {
                    ProductSet.Add(product);
                    _productNames.Add(product.Name, product);
                }
            };

            ProductManager.OnQueryAdd += query =>
            {
                dispatcher.Invoke(() => QueryQueue.Enqueue(query));
            };

            ProductManager.OnQuerySent += () => { dispatcher.Invoke(() => QueryQueue.Dequeue()); };

            ProductManager.OnQueryDbFail += msg =>
            {
                MessageBox.Show(msg, "The Failed Query");
            };

            ProductManager.StartWork();
        }

        public void AddProduct(string name, string number)
        {
            Tools.ValidateProductName(name);
            ProductManager.AddProduct(name, Tools.ParsePositiveNumber(number));
        }

        public void DeleteProduct(string name, string number)
        {
            Tools.ValidateProductName(name);
            ProductManager.DeleteProduct(name, Tools.ParsePositiveNumber(number));
        }

        public void GetStockInfo()
        {
            ProductManager.UpdateLocalProducts();
        }
    }
}