using System;
using System.Linq;
using System.Runtime.Caching;
using StackExchange.Redis;

namespace FibonacciNumbers
{
    /// <summary>
    /// Класс для работы с числами Фибоначчи.
    /// Числа Фибоначчи - элементы последовательности, в которой первые два числа равны либо 1 и 1, либо 0 и 1, а каждое последующее число равно сумме двух предыдущих чисел.
    /// Пример: 0 1 1 2 3 5 8 13 21 34 55 89 ...
    /// </summary>
    /// <remarks>Порядковый номер начинается с 0</remarks>
    public class Fibonacci
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IServer _server;
        private readonly IDatabase _database;
        private readonly ObjectCache _objectCach;

        private int _countCalc = 0;
        public int ValCountCalc { get { return _countCalc; } }

        public Fibonacci()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _server = _redis.GetServer(_redis.GetEndPoints().First());
            _database = _redis.GetDatabase();
            RedisCacheClear();

            _objectCach = MemoryCache.Default;
        }

        /// <summary>
        /// Метод, возвращающий число Фибоначчи по порядковуму номеру
        /// </summary>
        /// <returns>int</returns>
        public int GetValueByNumber(int n, CacheType cacheType)
        {
            string key = String.Format("Fibonacci{0}", n);

            if (cacheType.Equals(CacheType.RedisCaching))
                return GetValueByNumberFromRedisCache(n, key);
            else if (cacheType.Equals(CacheType.RuntimeCaching))
                return GetValueByNumberFromRuntimeCache(n, key);

            return 0;
        }

        /// <summary>
        /// Метод производит очистку Redis-кэша 
        /// </summary>
        public void RedisCacheClear()
        {
            Console.WriteLine("Запущена очистка Redis-кэша!");

            var keys = _server.Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("  - удален ключ {0}", key.ToString());
                _database.KeyDelete(key);
            }
        }

        /// <summary>
        /// Метод, возвращающий число Фибоначчи по порядковуму номеру, с попыткой поиска значения в кэше.
        /// Если значение в кэше ненайдено, производится вызов метода GetValueByNumber, для расчета значения.
        /// В качестве механизма кэширования используется StackExchange.Redis
        /// </summary>
        /// <returns>int</returns>
        private int GetValueByNumberFromRedisCache(int n, string key)
        {
            string res = _database.StringGet(key);

            if (res == null)
            {
                Console.WriteLine("Число Фибоначчи под номером {0}, не найдено в кэше. Производим расчет.", n);
                res = GetValueByNumber(n).ToString();
                _database.StringSet(key, res);
                Console.WriteLine("Число получено и записано в кэш.");
            }
            else { Console.WriteLine("Число Фибоначчи под номером {0}, найдено в кэше.", n); }

            return Int32.Parse(res);
        }

        /// <summary>
        /// Метод, возвращающий число Фибоначчи по порядковуму номеру, с попыткой поиска значения в кэше.
        /// Если значение в кэше ненайдено, производится вызов метода GetValueByNumber, для расчета значения.
        /// В качестве механизма кэширования используется System.Runtime.Caching
        /// </summary>
        /// <returns>int</returns>
        private int GetValueByNumberFromRuntimeCache(int n, string key)
        {
            object res = _objectCach.Get(key);

            if (res == null)
            {
                Console.WriteLine("Число Фибоначчи под номером {0}, не найдено в кэше. Производим расчет.", n);
                res = GetValueByNumber(n);
                _objectCach.Set(key, res, ObjectCache.InfiniteAbsoluteExpiration);
                Console.WriteLine("Число получено и записано в кэш.");
            }
            else { Console.WriteLine("Число Фибоначчи под номером {0}, найдено в кэше.", n); }

            return (int)res;
        }

        /// <summary>
        /// Метод, вычисляющий число Фибоначчи по порядковуму номеру.
        /// </summary>
        /// <returns>int</returns>
        private int GetValueByNumber(int n)
        {
            _countCalc++;

            if (n == 0) return 0;
            if (n == 1 || n == 2) return 1;

            int prevRes = 1;
            int res = 1;
            for (int i = 2; i < n; i++)
            {
                res = res + prevRes;
                prevRes = res - prevRes;
            }

            return res;
        }
    }

    public enum CacheType { RedisCaching, RuntimeCaching }
}
