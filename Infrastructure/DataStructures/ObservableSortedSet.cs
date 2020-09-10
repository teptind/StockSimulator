﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ProductsCounting.Infrastructure.DataStructures {
    public class ObservableSortedSet<T> : SortedSet<T>, INotifyCollectionChanged where T : IComparable<T> {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Add(T element) {
            base.Add(element);
            Update();
        }

        public void Update()
        {
            CollectionChanged?.Invoke(
                this, 
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

         public override void Clear() {
             base.Clear();
             Update();
        }

        public new void Remove(T element) {
            base.Remove(element);
            Update();
        }
    }
}
