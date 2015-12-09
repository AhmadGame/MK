namespace Mk.Stores
{
    public class Store
    {
        public Store()
        {
            ConnectionString = "Host=localhost;Username=postgres;Password=BevHills90210;Database=mk";
        }

        public string ConnectionString { get; }

    }
}
