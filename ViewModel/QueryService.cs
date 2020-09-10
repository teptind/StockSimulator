using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using ProductsCounting.Infrastructure.DataStructures;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;
using ProductsCounting.Service.Db;

namespace ProductsCounting.ViewModel {
    public class QueryService
    {
        public Action<string> OnOperationDeny;
        public ObservableConcurrentQueue<Query> Queries { get; } = new ObservableConcurrentQueue<Query>();

        // ConcurrentQueue<>
        public bool IsActive { get; set; } = true;

        public void Run()
        {
            while (IsActive)
            {
                // Where is connection exceptions???
                if (!Queries.IsEmpty && Queries.TryPeek(out var curr))
                {
                    try
                    {
                        if (curr.Type == Query.QueryType.Add)
                        {
                            StockController.AddProduct(curr.Source);
                        }
                        else if (curr.Type == Query.QueryType.Delete)
                        {
                            StockController.DeleteProduct(curr.Source);
                        }
                        Queries.Dequeue();
                        Trace.WriteLine($"{curr.TypeString} {curr.Source.Name} {curr.Source.Number} - ok");
                    }
                    catch (ManagerException e)
                    {
                        Trace.WriteLine($"{curr.TypeString} {curr.Source.Name} {curr.Source.Number} - bad");
                    }
                }
                Thread.Sleep(200);
            }
        }

        public void AddQuery(Query q)
        {
            Queries.Enqueue(q);
        }
    }
}
