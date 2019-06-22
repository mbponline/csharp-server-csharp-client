using System;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{

    public interface IEntitySet<out T>
        where T : class, IDerivedEntity
    {
        List<IDerivedEntity> Items { get; }

        T NavigateSingle(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey);

        IEnumerable<T> NavigateMulti(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey);

        IEnumerable<T> NavigateAllRelated(IEnumerable<Dto> remoteDtos, string[] remoteEntityKey, string[] navigationKey);

        T FindByKey(Dto partialDto);

        T Find(Func<T, bool> predicate);

        IEnumerable<T> Filter(Func<T, bool> predicate);

        void DeleteEntity(IDerivedEntity derivedEntity);

        void DeleteAll();

        void Dispose();

        T UpdateEntity(Dto dto);

        void AttachEntitySet(List<Dto> dtos);
    }

}