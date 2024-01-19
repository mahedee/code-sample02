namespace ConcurrencyHandling.API.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string Name { get; set; }

        public decimal Salary { get; set; }

        public Guid RowVersion { get; set; }
    }
}
