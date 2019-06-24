using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace NavyBlueDtos
{

    internal class DatabaseOperations
    {
        private readonly string connectionString;
        private readonly Dialect dialect;

        public DatabaseOperations(string connectionString, Dialect dialect)
        {
            this.connectionString = connectionString;
            this.dialect = dialect;
        }

        // connectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=QualityControlDb04;Integrated Security=True;MultipleActiveResultSets=True", "System.Data.SqlClient"
        public IEnumerable<dynamic> ExecuteQuery1(string queryText)
        {
            return ExecuteQuery<dynamic>(queryText);
        }

        public IEnumerable<T> ExecuteQuery<T>(string queryText)
        {
            var rows = Enumerable.Empty<T>();
            switch (this.dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(this.connectionString))
                    {
                        sqlConnection.Open();
                        rows = sqlConnection.Query<T>(queryText);
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(this.connectionString))
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

        public IEnumerable<Dto> ExecuteQuery(string queryText)
        {
            var dtos = new List<Dto>();
            switch (this.dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    // Not developed yet.
                    throw new NotImplementedException();
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(this.connectionString))
                    {
                        MySqlDataReader rdr = null;
                        try
                        {
                            sqlConnection.Open();
                            var cmd = new MySqlCommand(queryText, sqlConnection);
                            rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                //var row = Enumerable.Range(0, rdr.FieldCount).ToDictionary(rdr.GetName, rdr.GetValue);
                                //var row = Enumerable.Range(0, rdr.FieldCount).ToDictionary(i => rdr.GetName(i), i => rdr.GetValue(i));
                                var dto = new Dto();
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {

                                    //var dotNetType = rdr.GetFieldType(i);
                                    //var sqlType = rdr.GetDataTypeName(i);
                                    //var specificType = rdr.GetProviderSpecificFieldType(i);
                                    dto.Add(rdr.GetName(i), new JValue(rdr.GetValue(i)));
                                }
                                dtos.Add(dto);
                            }
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                            }

                            if (sqlConnection != null)
                            {
                                sqlConnection.Close();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return dtos;
        }


        public int CountQuery(string queryText)
        {
            var countRows = default(long);
            switch (this.dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(this.connectionString))
                    {
                        sqlConnection.Open();
                        var rows = sqlConnection.Query(queryText).ToList();
                        countRows = rows[0].count;
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(this.connectionString))
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

        public dynamic CudQuery(string queryText)
        {
            dynamic result = null;
            switch (this.dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    using (var sqlConnection = new SqlConnection(this.connectionString))
                    {
                        sqlConnection.Open();
                        result = sqlConnection.Query(queryText).First();
                        sqlConnection.Close();
                    }
                    break;
                case Dialect.MYSQL:
                    using (var sqlConnection = new MySqlConnection(this.connectionString))
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