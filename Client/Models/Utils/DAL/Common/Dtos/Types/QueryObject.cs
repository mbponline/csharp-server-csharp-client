using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{

    public class QueryObject
    {
        public Dto[] Keys { get; set; }

        public string[] Select { get; set; }

        public string Filter { get; set; }

        public List<FilterExpand> FilterExpand { get; set; }

        // daca este prezent acest camp continand SQL text atunci,
        // in loc sa se foloseasca ... table AS it ..., la constructia query-ului,
        // se foloseste ... (customQueryTable) AS it ...
        // Acesta ar trebui sa produca aceleasi campuri
        public string CustomQueryTable { get; set; }

        public string[] OrderBy { get; set; }

        public string[] Expand { get; set; }

        public bool? Count { get; set; }

        public int? Skip { get; set; }

        public int? Top { get; set; }
    }

}