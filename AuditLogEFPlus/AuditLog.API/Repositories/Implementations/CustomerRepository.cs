using AuditLog.API.Models;
using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace AuditLog.API.Repositories.Implementations
{

    public class CustomerRepository : ICustomerRepository
    {
        private readonly AuditLogDBContext _context;

        public CustomerRepository(AuditLogDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            // Used for audit log
            var audit = new Z.EntityFramework.Plus.Audit();
            audit.CreatedBy = "mahedee";

            _context.Customers.Add(customer);
            try
            {
                // SaveChangesAsync(audit) will commit the changes to the database and save the audit logs
                await _context.SaveChangesAsync(audit);
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var audit = new Z.EntityFramework.Plus.Audit();
            audit.CreatedBy = "mahedee";
            var customerToUpdate = await _context.Customers.FindAsync(customer.Id);
            _context.Entry(customerToUpdate).CurrentValues.SetValues(customer);

            try
            {
                // SaveChangesAsync(audit) will commit the changes to the database and save the audit logs
                await _context.SaveChangesAsync(audit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return customer;
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var audit = new Z.EntityFramework.Plus.Audit();
            audit.CreatedBy = "mahedee";

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return null;
            }

            try
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync(audit);
            }
            catch (Exception)
            {
                throw;
            }

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }

}
