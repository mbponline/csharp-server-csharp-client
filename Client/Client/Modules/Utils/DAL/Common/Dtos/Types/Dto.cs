using System.Collections.Generic;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{

    public class Dto : Dictionary<string, object>
    {
        public Dto()
            : base()
        {
        }

        public void SetDefaultValues(MetadataCli.EntityType entityType)
        {
            foreach (var item in entityType.Properties)
            {
                this[item.Key] = item.Value.Default;

            }
        }
    }

}
