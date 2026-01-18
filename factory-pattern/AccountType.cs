namespace FactoryPattern
{
    /// <summary>
    /// Enumeration defining the types of accounts available
    /// Used by the factory to determine which concrete class to instantiate
    /// </summary>
    public enum AccountType
    {
        Savings,
        Checking,
        BusinessChecking // Example of easy extensibility
    }
}