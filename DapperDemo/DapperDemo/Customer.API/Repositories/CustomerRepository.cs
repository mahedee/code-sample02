using Dapper;
using Microsoft.Extensions.Configuration;
using Ordering.API.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration)
            : base(configuration)
        {

        }

        public Task<int> CreateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                var query = "SELECT * FROM CUSTOMERS";
                using (var connection = CreateConnection())
                {
                    return (await connection.QueryAsync<Customer>(query)).ToList();
                }
            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM CUSTOMER WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("Id", id, DbType.Int32);

                using (var connection = CreateConnection())
                {
                    return (await connection.QueryFirstOrDefaultAsync<Customer>(query, parameters));
                }
            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }

        public Task<int> UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
