using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace Hospital.Model.Model
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }


        //public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        //{            
        //     using (var connection = CreateConnection())
        //     {
        //         return await connection.QueryAsync<T>(sql, parameters);
        //     }                        
        //}

        //public async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        //{
        //    using (var connection = CreateConnection())
        //    {
        //        return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        //    }
        //}

        //public async Task<int> ExecuteAsync(string sql, object parameters = null)
        //{
        //    using (var connection = CreateConnection())
        //    {
        //        return await connection.ExecuteAsync(sql, parameters);
        //    }
        //}

    }
}
