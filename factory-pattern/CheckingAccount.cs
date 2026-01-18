using System;

namespace FactoryPattern
{
    /// <summary>
    /// Concrete implementation of a Checking Account
    /// Provides easier access to funds but typically lower interest rates
    /// </summary>
    public class CheckingAccount : IAccount
    {
        private decimal balance = 0;
        
        public string Withdraw(decimal amount)
        {
            // Checking accounts might allow overdrafts
            balance -= amount;
            return $"Withdrew ${amount}. New balance: ${balance}";
        }
        
        public string Deposit(decimal amount)
        {
            balance += amount;
            return $"Deposited ${amount}. New balance: ${balance}";
        }
        
        public double GetInterestRate()
        {
            return 0.1; // 0.1% interest rate for checking
        }
        
        public string GetAccountType()
        {
            return "Checking Account";
        }
    }
}