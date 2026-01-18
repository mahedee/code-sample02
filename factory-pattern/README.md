# Factory Design Pattern Implementation

This project demonstrates the Factory Design Pattern using a banking application example in C#.

## Pattern Overview

The Factory Design Pattern is a creational pattern that provides an interface for creating objects without specifying their exact classes. It centralizes object creation logic and enables loose coupling between client code and concrete implementations.

## Project Structure

- **IAccount.cs** - Common interface for all account types
- **AccountType.cs** - Enumeration defining available account types
- **SavingsAccount.cs** - Savings account implementation
- **CheckingAccount.cs** - Checking account implementation  
- **BusinessCheckingAccount.cs** - Business checking account implementation
- **AccountFactory.cs** - Factory class responsible for creating account instances
- **Program.cs** - Client application demonstrating pattern usage

## Key Benefits

- **Centralized object creation** - All instantiation logic in one place
- **Loose coupling** - Clients depend only on interfaces
- **Easy extensibility** - New account types can be added without modifying client code
- **Interface-based design** - Client code works exclusively with interfaces

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the application

## Adding New Account Types

To add a new account type:
1. Create a new class implementing `IAccount`
2. Add the new type to the `AccountType` enum
3. Update the factory's switch statement
4. No client code modification required!

This demonstrates the pattern's strength in supporting extensible design.