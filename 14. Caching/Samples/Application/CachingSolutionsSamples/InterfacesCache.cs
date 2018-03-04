using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
    public interface ITableCache
    {
        IEnumerable<T> Get<T>(string forUser) where T : new();
        void Set<T>(string forUser, IEnumerable<T> categories);
        void Set<T>(string forUser, IEnumerable<T> categories, CacheItemPolicy policy);
        void Remove(string forUser);
    }
    /*
    public interface ICategoriesCache
	{
		IEnumerable<Category> Get(string forUser);
        void Set(string forUser, IEnumerable<Category> categories);
        void Set(string forUser, IEnumerable<Category> categories, SqlDependency dependency);
    }

    public interface ICustomerCache
    {
        IEnumerable<Customer> Get(string forUser);
        void Set(string forUser, IEnumerable<Customer> customers);
    }

    public interface IEmployeeCache
    {
        IEnumerable<Employee> Get(string forUser);
        void Set(string forUser, IEnumerable<Employee> employees);
    }*/


}
