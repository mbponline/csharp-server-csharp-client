using System;
using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public interface IEntitySet<T>
        where T : class, IEntity
    {
        List<T> Items { get; }

        T NavigateSingle(IEntity remoteEntity, string[] remoteEntityKey, string[] navigationKey);

        IEnumerable<T> NavigateMulti(IEntity remoteEntity, string[] remoteEntityKey, string[] navigationKey);

        IEnumerable<T> NavigateAllRelated(IEnumerable<object> remoteEntities, string[] remoteEntityKey, string[] navigationKey);

        T FindByKey(T partialEntity);

        T Find(Func<T, bool> predicate);

        IEnumerable<T> Filter(Func<T, bool> predicate);

        void DeleteEntity(T entity);

        void DeleteAll();

        void Dispose();

        T UpdateEntity(T dto);

        void AttachEntitySet(List<T> dtos);
    }

}