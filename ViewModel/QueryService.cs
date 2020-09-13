using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using ProductsCounting.Infrastructure.DataStructures;
using ProductsCounting.Infrastructure.Exceptions;
using ProductsCounting.Model;
using ProductsCounting.Service.Db;

namespace ProductsCounting.ViewModel
{
    public class QueryService
    {
        public Action<string> OnOperationDeny;
        public Action OnQuerySent;
        public ConcurrentQueue<Query> Queries { get; } = new ConcurrentQueue<Query>();

        // ConcurrentQueue<>
        public bool IsActive { get; set; } = true;

        public void Run()
        {
            while (IsActive)
            {
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

                        if (Queries.TryDequeue(out _))
                        {
                            OnQuerySent?.Invoke();
                        }

                        Trace.WriteLine($"{curr.TypeString} {curr.Source.Name} {curr.Source.Number} - ok");
                    }
                    catch (Exception e)
                    {
                        if (e is ManagerException)
                        {
                            Queries.TryDequeue(out _);
                            OnQuerySent?.Invoke();
                            Trace.WriteLine(
                                $"{curr.TypeString} {curr.Source.Name} {curr.Source.Number} - bad - {e.Message}");
                            OnOperationDeny?.Invoke(
                                $"Bad query: {curr.TypeString} {curr.Source.Name} {curr.Source.Number}. Reason: {e.Message}");
                        }
                        else
                        {
                            Thread.Sleep(200);
                            Trace.WriteLine(
                                $"{curr.TypeString} {curr.Source.Name} {curr.Source.Number} - resent - {e.Message}");
                        }
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