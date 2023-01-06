namespace Accounting.API.Models
{
    public class TransactionDetails
    {
        public Int64 Id { get; set; }

        // Master transaction Id
        public Int64 TransactionId { get; set; }
        
        // Id against GL Code
        public int AccountId { get; set; } 

        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }

        // Cash = 1, Cheque = 2
        public int TransactionType { get; set; }

        // Any note
        public string Description { get; set; }

    }
}
