using System;
using System.Collections.Generic;

namespace ECommerceSample.Data.Repository
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(T entity);
        T GetById(object id);
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
