using System.Collections.Generic;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{

    public static class NavigationHelperUtils
    {
        // Utilizat doar de 'Navigation.ts', metoda fictiva 'select()'
        // este folosita doar pentru intellisense pentru generarea expand string[].
        public static T Select<T>(this IEnumerable<T> entities)
            where T : class, IDerivedEntity
        {
            return entities.FirstOrDefault();
        }
    }

}
