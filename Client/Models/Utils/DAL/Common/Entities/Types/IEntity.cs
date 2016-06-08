using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{
    public interface IEntity
    {
        void _attach(Dictionary<string, IEntitySet<IEntity>> entitySet, Metadata metadata);

        void _detach();
    }
}