using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Tools.Modules.Common.Database
{

    internal class DatabaseOperations
    {
        private string _dialect;
        private string _connectionString;

        public DatabaseOperations(string dialect, string connectionString)
        {
            this._dialect = dialect;
            this._connectionString = connectionString;
        }

        public IEnumerable<T> Query<T>(string queryText)
        {
            var rows = Enumerable.Empty<T>();
            switch (this._dialect)
            {
                case "MSSQL":
                    using (var sqlConnection = new SqlConnection(this._connectionString))
                    {
                        sqlConnection.Open();
                        rows = sqlConnection.Query<T>(queryText);
                        sqlConnection.Close();
                    }
                    break;
                case "MYSQL":
                    using (var sqlConnection = new MySqlConnection(this._connectionString))
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


    }

}