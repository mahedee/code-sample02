using HRM.API.Models;

namespace HRM.API.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetEmployee(int id);
        public Task<string> EditEmployee(int id, Employee employee);
        public Task<string> AddEmployee(Employee employee);
        public Task<string> RemoveEmployee(int id);
    }
}
