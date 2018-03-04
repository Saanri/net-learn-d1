using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
    public class DataManager
    {
        private ITableCache _cache;

        private int _type;
        private string _connectionStrings;
        private string _commandString;

        public DataManager(ITableCache cache)
        {
            _cache = cache;
            _type = 0;
        }

        public DataManager(ITableCache cache, string connectionStrings, string commandString)
        {
            _cache = cache;
            _type = 1;

            _connectionStrings = connectionStrings;
            _commandString = commandString;
        }

        public IEnumerable<T> GetData<T>() where T : class, new()
        {
            Console.WriteLine("Get data");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var data = _cache.Get<T>(user);

            if (data == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;

                    data = dbContext.Set<T>().ToList();

                    if (_type == 0) _cache.Set(user, data);
                    else if (_type == 1)
                    {
                        CacheItemPolicy policy = new CacheItemPolicy();
                        SqlDependency.Start(_connectionStrings);
                        using (SqlConnection connection = new SqlConnection(_connectionStrings))
                        using (SqlCommand command = new SqlCommand(_commandString, connection))
                        {
                            connection.Open();

                            SqlDependency dependency = new SqlDependency(command);
                            dependency.OnChange += dependency_OnChange;
                            SqlChangeMonitor monitor = new SqlChangeMonitor(dependency);

                            command.ExecuteReader();

                            policy.ChangeMonitors.Add(monitor);

                        }
                        
                        _cache.Set(user, data, policy);
                    }
                }
            }

            return data;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            _cache.Remove(user);
        }

    }
}

