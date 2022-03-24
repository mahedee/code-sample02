using HRM.API.Models;

namespace HRM.API.Repository
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<Employee>> SelectAllEmployees();
        public Task<Employee> SelectEmployee(int id);
        public Task<string> UpdateEmployee(int id, Employee employee);
        public Task<string> SaveEmployee(Employee employee);
        public Task<string> DeleteEmployee(int id);
    }
}
