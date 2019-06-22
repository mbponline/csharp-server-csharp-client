using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{

    internal static class DatabaseOperations
    {
        // connectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=QualityControlDb04;Integrated Security=True;MultipleActiveResultSets=True", "System.Data.SqlClient"
        public static IEnumerable<dynamic> ExecuteQuery(string queryText, Dialect dialect, string connectionString)
        {
            return ExecuteQuery<dynamic>(queryText, dialect, connectionString);
        }

        public static IEnumerable<T> ExecuteQuery<T>(string queryText, Dialect dialect, string connectionString)
        {
            var rows = Enumerable.Empty<T>();
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        rows = sqlConnection.Query<T>(queryText);
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        rows = sqlConnection.Query<T>(queryText);
                        sqlConnection.Close();
                    }
                    break;
                default:
                    break;
            }

            return rows;
        }

        public static int CountQuery(string queryText, Dialect dialect, string connectionString)
        {
            var countRows = default(long);
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var rows = sqlConnection.Query(queryText).ToList();
                        countRows = rows[0].count;
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var rows = sqlConnection.Query(queryText).ToList();
                        countRows = rows[0].count;
                        sqlConnection.Close();
                    }
                    break;
                default:
                    break;
            }
            return (int)countRows;
        }

        public static dynamic CudQuery(string queryText, Dialect dialect, string connectionString)
        {
            dynamic result = null;
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        result = sqlConnection.Query(queryText).First();
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        result = sqlConnection.Query(queryText).First();
                        sqlConnection.Close();
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }

}