using System.Data;
using Dapper;

namespace CleanArch.Application.Common.Interfaces.Repositories.Query.Base
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get All Data using SQL Query or procedure without dapper parameters from single entity
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(string sql, bool isProcedure = false);

        /// <summary>
        /// Get All data using SQL Query or Procedure with dapper parameters from a single entity
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(string sql, DynamicParameters parameters, bool isProcedure = false);

        /// <summary>
        /// Get All data using SQL Query or Procedure without dapper parameters from two entities
        ///
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TEntity>(string sql, Func<TFirst, TSecond, TEntity> map, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from two entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TEntity>(string sql, Func<TFirst, TSecond, TEntity> map, DynamicParameters parameters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure without dapper parameters from three entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TEntity>(string sql, Func<TFirst, TSecond, TThird, TEntity> map, string splitters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from three entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TEntity>(string sql, Func<TFirst, TSecond, TThird, TEntity> map, DynamicParameters parameters, string splitters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure without dapper parameters from four entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TEntity> map, string splitters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from four entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TEntity> map, DynamicParameters parameters, string splitters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure without dapper parameters from five entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TEntity> map, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from five entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TEntity> map, DynamicParameters parameters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TEntity : class;



        /// <summary>
        ///  Get All data using SQL Query or Procedure without dapper parameters from six entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity> map, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from six entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity> map, DynamicParameters parameters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TEntity : class;


        /// <summary>
        ///  Get All data using SQL Query or Procedure without dapper parameters from seven entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TSeventh"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity> map, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TSeventh : class
            where TEntity : class;

        /// <summary>
        ///  Get All data using SQL Query or Procedure with dapper parameters from seven entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name= "TSeventh"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity> map, DynamicParameters parameters, bool isProcedure = false)
            where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TSeventh : class
            where TEntity : class;

        Task<TEntity> SingleAsync(string sql, bool isProcedure = false);

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from single entity
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(string sql, DynamicParameters parameters, bool isProcedure = false);

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from two entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TEntity>(string sql, Func<TFirst, TSecond, TEntity> map, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure with dapper parameters from two entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity?> SingleAsync<TFirst, TSecond, TEntity>(string sql, Func<TFirst, TSecond, TEntity> map, DynamicParameters parameters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from three entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TEntity>(string sql, Func<TFirst, TSecond, TThird, TEntity> map, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure with dapper parameters from three entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TEntity>(string sql, Func<TFirst, TSecond, TThird, TEntity> map, DynamicParameters parameters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from four entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TEntity> map, string splitters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure with dapper parameters from four entity
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity?> SingleAsync<TFirst, TSecond, TThird, TFourth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TEntity> map, DynamicParameters parameters, string splitters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from five entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TEntity> map, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from five entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TEntity> map, DynamicParameters parameters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from six entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity> map, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure with dapper parameters from six entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TEntity> map, DynamicParameters parameters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure without dapper parameters from seven entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TSeventh"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity> map, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TSeventh : class
            where TEntity : class;

        /// <summary>
        /// Get Single Mapped Data using SQL Query or Procedure with dapper parameters from seven entities
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TSeventh"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEntity> map, DynamicParameters parameters, bool isProcedure = false) where TFirst : class
            where TSecond : class
            where TThird : class
            where TFourth : class
            where TFifth : class
            where TSixth : class
            where TSeventh : class
            where TEntity : class;

        /// <summary>
        /// Get DataTable using SQl Query or Procedure without dapper parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<DataTable> GetDataTableAsync(string sql, bool isProcedure = false);
        /// <summary>
        /// Get DataTable using SQL Query or Procedure with adding dapper parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isProcedure"></param>
        /// <returns></returns>
        Task<DataTable> GetDataTableAsync(string sql, DynamicParameters parameters, bool isProcedure = false);
    }
}
