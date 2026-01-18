using System;

namespace FactoryPattern
{
    /// <summary>
    /// Concrete implementation of a Savings Account
    /// Provides higher interest rates but may have withdrawal limitations
    /// </summary>
    public class SavingsAccount : IAccount
    {
        private decimal balance = 0;
        
        public string Withdraw(decimal amount)
        {
            if (amount <= balance)
            {
                balance -= amount;
                return $"Withdrew ${amount}. New balance: ${balance}";
            }
            return "Insufficient funds for withdrawal.";
        }
        
        public string Deposit(decimal amount)
        {
            balance += amount;
            return $"Deposited ${amount}. New balance: ${balance}";
        }
        
        public double GetInterestRate()
        {
            return 2.5; // 2.5% interest rate for savings
        }
        
        public string GetAccountType()
        {
            return "Savings Account";
        }
    }
}