using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{

    public class Dto : Dictionary<string, object>
    {
        public Dto()
            : base()
        {
        }

        public void SetDefaultValues(EntityType entityType)
        {
            foreach (var item in entityType.Properties)
            {
                this[item.Key] = item.Value.Default;

            }
        }
    }

}
