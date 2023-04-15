using System;
using System.Collections.Generic;
using System.Linq;

namespace SingletonDemo
{

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public sealed class SingletonEmployeeService
    {
        // singleton static object
        private static SingletonEmployeeService instance;

        // an object to use in a singleton class
        private List<Employee> employees = null;

        // Restrict to create object of singleton class
        private SingletonEmployeeService()
        {
            if(employees == null)
            {
                employees = new List<Employee>();
            }
        }

        // The static method to provide global access to the singleton object
        // Get singleton object of SingletonEmployeeService class
        public static SingletonEmployeeService GetInstance()
        {
            if (instance == null)
            {
                // Thread safe singleton
                lock (typeof(SingletonEmployeeService))
                {
                    instance = new SingletonEmployeeService();
                }
            }
            return instance;
        }

        // Add employee to the employees list
        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        // Get name of the employee by Id

        public string GetEmployeeName(int id) {
        
            string name = employees.Where(p => p.Id == id).First().Name;
            return name;
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {

            Employee emp1 = new Employee { Id = 1, Name = "John", Age = 30 };
            Employee emp2 = new Employee { Id = 2, Name = "Abraham", Age = 40};
            

            // Create singleton instance using GetInstance method not new
            SingletonEmployeeService singletonEmployeeService = SingletonEmployeeService.GetInstance();
            
            singletonEmployeeService.AddEmployee(emp1);
            singletonEmployeeService.AddEmployee(emp2);

            Console.WriteLine(singletonEmployeeService.GetEmployeeName(2));

            Console.ReadKey();
        }
    }
}

// Output:
// Abraham