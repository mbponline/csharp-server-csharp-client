using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{

    public class QueryObject
    {
        public Dto[] Keys { get; set; }

        public string[] Select { get; set; }

        public string Filter { get; set; }

        public FilterExpand[] FilterExpand { get; set; }

        public string[] OrderBy { get; set; }

        public string[] Expand { get; set; }

        public bool? Count { get; set; }

        public int? Skip { get; set; }

        public int? Top { get; set; }
    }

}