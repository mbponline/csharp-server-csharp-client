using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NavyBlueDtos
{

    public class Dto : Dictionary<string, JValue>
    {
        public Dto()
            : base()
        {
        }

        public void SetDefaultValues(MetadataSrv.EntityType entityType)
        {
            foreach (var item in entityType.Properties)
            {
                this[item.Key] = new JValue(item.Value.Default);

            }
        }
    }

}