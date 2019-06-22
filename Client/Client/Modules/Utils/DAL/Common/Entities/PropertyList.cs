using System;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{
    public class PropertyList : Dictionary<string, object>
    {
        public PropertyList(params object[] parameters)
        {
            this.parameters = parameters;
        }

        private readonly object[] parameters;

        protected T GetPropertyValue<T>(/*string propertyName*/)
        {
            var entityType = typeof(T);
            var propertyName = entityType.GenericTypeArguments[0].Name;
            T instance;
            if (this.ContainsKey(propertyName))
            {
                instance = (T)this[propertyName];
            }
            else
            {
                instance = (T)Activator.CreateInstance(entityType, this.parameters);
                this[propertyName] = instance;
            }
            return instance;
        }
    }
}