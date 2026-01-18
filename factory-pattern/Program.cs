using System;

namespace FactoryPattern
{
    /// <summary>
    /// Client application demonstrating the Factory Pattern usage
    /// Notice how the client code is completely decoupled from concrete implementations
    /// </summary>
    public class BankingApplication
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Banking System - Factory Pattern Demo ===\n");
            
            try
            {
                // Create different types of accounts using the factory
                IAccount savingsAccount = AccountFactory.CreateAccount(AccountType.Savings);
                IAccount checkingAccount = AccountFactory.CreateAccount(AccountType.Checking);
                IAccount businessAccount = AccountFactory.CreateAccount(AccountType.BusinessChecking);
                
                // Alternative: Create account using string input (useful for user interfaces)
                IAccount userAccount = AccountFactory.CreateAccount("Savings");
                
                // Use the accounts without knowing their concrete types
                DisplayAccountInfo(savingsAccount);
                DisplayAccountInfo(checkingAccount);
                DisplayAccountInfo(businessAccount);
                
                // Perform operations
                Console.WriteLine("\n=== Account Operations ===");
                Console.WriteLine(savingsAccount.Deposit(1000));
                Console.WriteLine(savingsAccount.Withdraw(250));
                
                Console.WriteLine(checkingAccount.Deposit(500));
                Console.WriteLine(checkingAccount.Withdraw(600)); // This might overdraft
                
                Console.WriteLine(businessAccount.Deposit(2000));
                Console.WriteLine(businessAccount.Withdraw(500)); // This will include fees
                
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Helper method to display account information
        /// Demonstrates working with objects through their interface
        /// </summary>
        /// <param name="account">Account instance to display information for</param>
        private static void DisplayAccountInfo(IAccount account)
        {
            Console.WriteLine($"Account Type: {account.GetAccountType()}");
            Console.WriteLine($"Interest Rate: {account.GetInterestRate()}%");
            Console.WriteLine();
        }
    }
}