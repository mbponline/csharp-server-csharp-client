using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public class QueryParams
    {
        public string Keys { get; set; }

        public string Select { get; set; }

        public string Filter { get; set; }

        public string FilterExpand { get; set; }

        public string OrderBy { get; set; }

        public string Expand { get; set; }

        public string Count { get; set; }

        public string Skip { get; set; }

        public string Top { get; set; }
    }

}