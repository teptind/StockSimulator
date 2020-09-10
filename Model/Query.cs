namespace ProductsCounting.Model {
    public class Query {
        public enum QueryType
        {
            Add,
            Delete
        }

        public QueryType Type { get; }
        public Product Source { get; }

        public string TypeString { get; }

        public Query(QueryType type, Product product)
        {
            Type = type;
            Source = product;
            TypeString = type.ToString();
        }
    }
}
