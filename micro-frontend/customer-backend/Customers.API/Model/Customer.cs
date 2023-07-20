namespace Customers.API.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNo { get; set;}
        public string? EmailAddress { get; set;}
        public DateTime? DOB { get; set; }
    }
}
