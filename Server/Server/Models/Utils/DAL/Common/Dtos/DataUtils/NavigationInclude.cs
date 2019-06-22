using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public class NavigationInclude
    {
        public string NavigationProperty { get; set; }

        public List<NavigationInclude> Include { get; set; }

        public string Filter { get; set; }
    }

}