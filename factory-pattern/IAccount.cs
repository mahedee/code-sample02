using System;

namespace FactoryPattern
{
    /// <summary>
    /// Common interface for all account types
    /// Defines the contract that all account implementations must follow
    /// </summary>
    public interface IAccount
    {
        string Withdraw(decimal amount);
        string Deposit(decimal amount);
        double GetInterestRate();
        string GetAccountType();
    }
}