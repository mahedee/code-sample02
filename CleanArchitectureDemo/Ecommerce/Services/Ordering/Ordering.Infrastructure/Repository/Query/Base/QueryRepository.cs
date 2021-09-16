using Microsoft.Extensions.Configuration;
using Ordering.Core.Repositories.Query.Base;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository.Query.Base
{
    public class QueryRepository<T> : DbConnector,  IQueryRepository<T> where T : class
    {
        public QueryRepository(IConfiguration configuration)
            : base(configuration)
        {

        }

        //public Task<IReadOnlyList<T>> GetAllAsync()
        //{
        //    try
        //    {
        //        var query = "SELECT * FROM " + T.ToString();

        //        using (var connection = CreateConnection())
        //        {
        //            return (await connection.QueryAsync<Customer>(query)).ToList();
        //        }
        //    }
        //    catch(Exception exp)
        //    {
        //        throw new Exception(exp.Message, exp);
        //    }
 
        //    throw new NotImplementedException();
        //}

        //public Task<T> GetByIdAsync(long id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
