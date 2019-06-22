using System;

namespace Server.Models.Utils.DAL.Common
{

    public enum Dialect
    {
        SQL2012,
        SQL2014,
        MYSQL,
    }

    public static class DialectUtils
    {
        public static Dialect Dialect(this Metadata metadata)
        {
            // Minimum SQL 2012 because it has improved paging FETCH NEXT ... OFFSET ...
            if (metadata.Database.Dialect == "MSSQL" && metadata.Database.Version == 11)
            {
               return Common.Dialect.SQL2012;
            }
            else if (metadata.Database.Dialect == "MSSQL" && metadata.Database.Version == 12)
            {
                return Common.Dialect.SQL2014;
            }
            else if (metadata.Database.Dialect == "MYSQL" && metadata.Database.Version >= 5)
            {
                return Common.Dialect.MYSQL;
            }
            else
            {
                throw new ArgumentException("Unsupported database");
            }
        }
    }
}