using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Threading;

namespace ProductsCounting.Infrastructure.DataStructures {
    public class ObservableConcurrentQueue<T> : ConcurrentQueue<T>, INotifyCollectionChanged {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public new void Enqueue(T element) {
            base.Enqueue(element);
            Update();
        }

        public void Update() {
            // CollectionChanged?.Invoke(
            //     this,
            //     new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Dequeue() {
            base.TryDequeue(out _);
            Update();
        }
    }
}
