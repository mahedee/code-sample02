namespace StockNotifications
{
    // Interface for the observer
    public interface IObserver
    {
        void Update(string symbol, decimal price);
    }

    // Interface for the subject
    public interface IStock
    {
        void Register(IObserver observer);
        void Unregister(IObserver observer);
        void Notify();
    }

    // Concrete subject
    public class Stock : IStock
    {
        private string _symbol;
        private decimal _price;
        private List<IObserver> _observers = new List<IObserver>();

        public Stock(string symbol, decimal price)
        {
            _symbol = symbol;
            _price = price;
        }

        public void Register(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unregister(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // If the price changes, notify the observers (traders)
        // Here we are calling the Notify method
        public void UpdatePrice(decimal price)
        {
            _price = price;
            Notify();
        }
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_symbol, _price);
            }
        }
    }

    // Concrete observer
    public class Trader : IObserver
    {
        // The observer's name
        private string _name;

        public Trader(string name)
        {
            _name = name;
        }
        public void Update(string symbol, decimal price)
        {
            Console.WriteLine($"Notified {_name} of {symbol}'s price change to {price}");
        }
    }
    public class Program
    {

        static void Main(string[] args)
        {
            // Create a stock
            Stock stock = new Stock("MSFT", 100.00m);

            // Create some traders
            Trader joe = new Trader("Joe");
            Trader sally = new Trader("Sally");

            // Register/subscribe the traders
            Console.WriteLine("Registering Joe and Sally");
            stock.Register(joe);
            stock.Register(sally);

            Console.WriteLine("Changing stock price to 105.00m");
            // Change the stock price
            stock.UpdatePrice(105.00m);

            // Change the stock price
            Console.WriteLine("Changing stock price to 110.00m");
            stock.UpdatePrice(110.00m);

            // Unregister/unsubscribe a trader
            Console.WriteLine("Unregistering Joe");
            stock.Unregister(joe);

            Console.WriteLine("Changing stock price to 115.00m");
            // Change the stock price
            stock.UpdatePrice(115.00m);
        }
    }
}
