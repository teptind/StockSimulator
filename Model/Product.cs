using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProductsCounting.Model
{
    public class Product : IComparable<Product>
    {
        public string Name { get; }

        public int Number { get; set; }

        public Product(int number, string name)
        {
            Number = number;
            Name = name;
        }

        public static SortedSet<Product> GetMockProducts()
        {
            return new SortedSet<Product>()
            {
                new Product(12, "Eggs"),
                new Product(5, "VodkaBelenkaya"),
                new Product(1, "Bread"),
                new Product(1, "Mince(kg)"),
                new Product(1, "Cheese"),
                new Product(1, "Apple"),
                new Product(1, "Mouse"),
                new Product(1, "Milk"),
                new Product(1, "Chopper"),
                new Product(1, "Snickers"),
                new Product(1, "Dynamite"),
            };
        }

        public int CompareTo([AllowNull] Product other)
        {
            return other == null ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}