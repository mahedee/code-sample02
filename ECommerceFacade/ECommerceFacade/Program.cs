namespace ECommerceFacade
{
    // Define Subsystems
    public class InventorySystem
    {
        public bool CheckStock(string item)
        {
            Console.WriteLine($"Checking stock for {item}...");
            return true; // Assume stock is available
        }
        public void UpdateInventory()
        {
            Console.WriteLine("Updating inventory...");
        }
    }

    public class PaymentGateway
    {
        public bool ProcessPayment(string cardDetails, decimal amount)
        {
            Console.WriteLine($"Processing payment of ${amount} using {cardDetails}...");
            return true; // Assume payment is successful
        }
    }


    public class ShippingSystem
    {
        public void ScheduleDelivery(string item, string address)
        {
            Console.WriteLine($"Scheduling delivery of {item} to {address}...");
        }
    }

    public class NotificationService
    {
        public void SendOrderConfirmation(string email)
        {
            Console.WriteLine($"Sending order confirmation email to {email}...");
        }
    }


    // Define Facade
    public class OrderProcessingFacade
    {
        private readonly InventorySystem _inventorySystem;
        private readonly PaymentGateway _paymentGateway;
        private readonly ShippingSystem _shippingSystem;
        private readonly NotificationService _notificationService;

        public OrderProcessingFacade()
        {
            _inventorySystem = new InventorySystem();
            _paymentGateway = new PaymentGateway();
            _shippingSystem = new ShippingSystem();
            _notificationService = new NotificationService();
        }

        public void PlaceOrder(string item, string cardDetails, decimal amount, string address, string email)
        {
            Console.WriteLine("Starting order processing...");

            if (!_inventorySystem.CheckStock(item))
            {
                Console.WriteLine("Item is out of stock.");
                return;
            }

            _inventorySystem.UpdateInventory();

            if (!_paymentGateway.ProcessPayment(cardDetails, amount))
            {
                Console.WriteLine("Payment failed.");
                return;
            }

            _shippingSystem.ScheduleDelivery(item, address);
            _notificationService.SendOrderConfirmation(email);

            Console.WriteLine("Order processed successfully!");
        }
    }


    // Client
    public class Program
    {
        static void Main(string[] args)
        {
            var orderFacade = new OrderProcessingFacade();

            // Order details
            string item = "Laptop";
            string cardDetails = "1234-5678-9876-5432";
            decimal amount = 1200.00m;
            string address = "123 Main St, Toronto, ON";
            string email = "customer@example.com";

            // Place the order
            orderFacade.PlaceOrder(item, cardDetails, amount, address, email);
        }
    }
}
