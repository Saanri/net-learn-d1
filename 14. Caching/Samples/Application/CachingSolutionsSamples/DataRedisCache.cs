using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Threading;

namespace CachingSolutionsSamples
{
    class DataRedisCache<T> : ITableCache
    {
        private ConnectionMultiplexer redisConnection;
        private readonly string _prefix = "CacheRedis_";
        private readonly string _nameCache;
        private readonly DataContractSerializer serializer =
            new DataContractSerializer(typeof(IEnumerable<T>));

        public DataRedisCache(string hostName, string nameCache)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
            _nameCache = nameCache;

            var database = redisConnection.GetDatabase();
            var server = redisConnection.GetServer(redisConnection.GetEndPoints().First());
            var keys = server.Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("  - удален ключ {0}", key.ToString());
                database.KeyDelete(key);
            }

        }

        public IEnumerable<T> Get<T>(string forUser) where T : new()
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(_prefix + _nameCache + forUser);
            if (s == null)
                return null;

            return (IEnumerable<T>)serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set<T>(string forUser, IEnumerable<T> data)
        {
            var db = redisConnection.GetDatabase();
            var key = _prefix + _nameCache + forUser;

            if (data == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, data);
                db.StringSet(key, stream.ToArray(), TimeSpan.FromSeconds(0.1));
            }
        }

        public void Set<T1>(string forUser, IEnumerable<T1> data, CacheItemPolicy policy)
        {
            var db = redisConnection.GetDatabase();
            var key = _prefix + _nameCache + forUser;

            if (data == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, data);
                db.StringSet(key, stream.ToArray());
            }
        }

        public void Remove(string forUser)
        {
            redisConnection.GetDatabase().KeyDelete(_prefix + _nameCache + forUser);
        }
    }
}
