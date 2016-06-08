using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{
    public class QueryResult<T>
    {
        public IEnumerable<T> Rows { get; set; }

        public int TotalRows { get; set; }
    }
}