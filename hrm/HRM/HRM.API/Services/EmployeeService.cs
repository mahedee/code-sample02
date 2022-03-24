using HRM.API.Models;
using HRM.API.Repository;

namespace HRM.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository repository)
        {
            _employeeRepository = repository;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                return await _employeeRepository.SelectAllEmployees();
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            try
            {
                return await _employeeRepository.SelectEmployee(id);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<string> EditEmployee(int id, Employee employee)
        {
            try
            {
                return await _employeeRepository.UpdateEmployee(id, employee);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<string> AddEmployee(Employee employee)
        {
            try
            {
                return await _employeeRepository.SaveEmployee(employee);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        public async Task<string> RemoveEmployee(int id)
        {
            try
            {
                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

    }
}
