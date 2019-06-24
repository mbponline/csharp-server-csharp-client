using System;

namespace NavyBlueDtos
{

    public enum Dialect
    {
        SQL2012,
        SQL2014,
        MYSQL,
    }

    public static class DialectUtils
    {
        public static Dialect Dialect(this MetadataSrv.Metadata metadataSrv)
        {
            // Minimum SQL 2012 because it has improved paging FETCH NEXT ... OFFSET ...
            if (metadataSrv.Dialect == "MSSQL11")
            {
                return NavyBlueDtos.Dialect.SQL2012;
            }
            else if (metadataSrv.Dialect == "MSSQL12")
            {
                return NavyBlueDtos.Dialect.SQL2014;
            }
            else if (metadataSrv.Dialect == "MYSQL")
            {
                return NavyBlueDtos.Dialect.MYSQL;
            }
            else
            {
                throw new ArgumentException("Unsupported database");
            }
        }
    }
}