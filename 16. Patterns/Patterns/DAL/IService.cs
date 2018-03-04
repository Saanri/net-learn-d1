using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IService<T,R> : IDisposable
        where T : class
        where R : class
    {
        List<T> GetAllByUrl(string url);
        List<R> ConvertForDb(List<T> list);
        void SaveAllToDb(List<R> list);
    }
}
