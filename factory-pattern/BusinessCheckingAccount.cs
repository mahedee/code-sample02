namespace FactoryPattern
{
    /// <summary>
    /// New account type demonstrating pattern extensibility
    /// Added without modifying any existing client code
    /// </summary>
    public class BusinessCheckingAccount : IAccount
    {
        private decimal balance = 0;
        
        public string Withdraw(decimal amount)
        {
            // Business accounts might have different fee structures
            decimal fee = 2.50m; // Transaction fee
            decimal totalAmount = amount + fee;
            
            if (totalAmount <= balance)
            {
                balance -= totalAmount;
                return $"Withdrew ${amount} (${fee} fee). New balance: ${balance}";
            }
            return "Insufficient funds for withdrawal including fees.";
        }
        
        public string Deposit(decimal amount)
        {
            balance += amount;
            return $"Business deposit: ${amount}. New balance: ${balance}";
        }
        
        public double GetInterestRate()
        {
            return 0.05; // Lower rate but business-focused features
        }
        
        public string GetAccountType()
        {
            return "Business Checking Account";
        }
    }
}