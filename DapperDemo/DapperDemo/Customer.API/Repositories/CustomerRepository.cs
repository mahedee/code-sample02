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
    public class CustomerRepository : DbConnector, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<int> CreateAsync(Customer entity)
        {
            try
            {
                var query = "INSERT INTO CUSTOMERS (FIRSTNAME, LASTNAME, EMAIL, CONTACTNUMBER, ADDRESS) " +
                    "VALUES (@FIRSTNAME, @LASTNAME, @EMAIL, @CONTACTNUMBER, @ADDRESS)";

                var parameters = new DynamicParameters();
                parameters.Add("FIRSTNAME", entity.FirstName);
                parameters.Add("LASTNAME", entity.LastName);
                parameters.Add("EMAIL", entity.Email);
                parameters.Add("CONTACTNUMBER", entity.ContactNumber);
                parameters.Add("ADDRESS", entity.Address);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, parameters));
                }
            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }

        public async Task<int> DeleteAsync(Customer entity)
        {
            try
            {
                var query = "DELETE FROM CUSTOMERS WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("Id", entity.Id, DbType.Int64);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, parameters));
                }
            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
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


        public async Task<List<Customer>> GetAllByEmailId(string email)
        {
            try
            {
                var procedure = "spGetCustomersByEmail";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                using (var connection = CreateConnection())
                {
                    return (await connection.QueryAsync<Customer>(procedure, parameters, commandType: CommandType.StoredProcedure)).ToList();
                }

            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }

        public async Task<Customer> GetByIdAsync(Int64 id)
        {
            try
            {
                var query = "SELECT * FROM CUSTOMERS WHERE Id = @Id";
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

        public async Task<int> UpdateAsync(Customer entity)
        {
            try
            {
                var query = "UPDATE CUSTOMERS SET FIRSTNAME = @FIRSTNAME, LASTNAME = @LASTNAME, EMAIL = @EMAIL, CONTACTNUMBER = @CONTACTNUMBER, ADDRESS = @ADDRESS WHERE ID = @ID ";
                var parameters = new DynamicParameters();
                parameters.Add("FIRSTNAME", entity.FirstName, DbType.String);
                parameters.Add("LASTNAME", entity.LastName, DbType.String);
                parameters.Add("CONTACTNUMBER", entity.ContactNumber, DbType.String);
                parameters.Add("ADDRESS", entity.Address, DbType.String);
                parameters.Add("EMAIL", entity.Email, DbType.String);
                parameters.Add("ID", entity.Id, DbType.Int64);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, parameters));
                }
            }
            catch(Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }
        }
    }
}
