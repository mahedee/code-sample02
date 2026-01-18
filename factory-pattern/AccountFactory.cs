using System;

namespace FactoryPattern
{
    /// <summary>
    /// Factory class responsible for creating account instances
    /// Centralizes object creation logic and provides a single point of control
    /// </summary>
    public static class AccountFactory
    {
        /// <summary>
        /// Creates an account instance based on the specified type
        /// </summary>
        /// <param name="accountType">The type of account to create</param>
        /// <returns>An IAccount instance of the specified type</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid account type is provided</exception>
        public static IAccount CreateAccount(AccountType accountType)
        {
            return accountType switch
            {
                AccountType.Savings => new SavingsAccount(),
                AccountType.Checking => new CheckingAccount(),
                AccountType.BusinessChecking => new BusinessCheckingAccount(), // Future extension
                _ => throw new ArgumentException($"Unknown account type: {accountType}")
            };
        }
        
        /// <summary>
        /// Overloaded method that creates an account based on string input
        /// Useful for scenarios where account type comes from user input or configuration
        /// </summary>
        /// <param name="accountTypeName">String representation of the account type</param>
        /// <returns>An IAccount instance of the specified type</returns>
        public static IAccount CreateAccount(string accountTypeName)
        {
            if (Enum.TryParse<AccountType>(accountTypeName, true, out AccountType accountType))
            {
                return CreateAccount(accountType);
            }
            
            throw new ArgumentException($"Invalid account type name: {accountTypeName}");
        }
    }
}