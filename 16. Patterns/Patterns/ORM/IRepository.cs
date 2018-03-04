using System;
using System.Collections.Generic;

namespace ORM
{
    public interface IRepository<T> : IDisposable
            where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T item);
        void Remove(T item);
        void Update(T item);
    }
}
