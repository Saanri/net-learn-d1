using System;
using FibonacciNumbers;

namespace FibonacciNumbersConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Старт!");
            Fibonacci fibonacciGetter = new Fibonacci();

            Console.WriteLine();
            Console.WriteLine("Демонстрация работы Redis-кэша");
            Console.WriteLine(fibonacciGetter.GetValueByNumber(12, CacheType.RedisCaching));
            Console.WriteLine(fibonacciGetter.GetValueByNumber(12, CacheType.RedisCaching));
            Console.WriteLine("Общее кол-во вызовов метода расчета: {0}", fibonacciGetter.ValCountCalc);

            Console.WriteLine();
            Console.WriteLine("Демонстрация работы Runtime-кэша");
            Console.WriteLine(fibonacciGetter.GetValueByNumber(10, CacheType.RuntimeCaching));
            Console.WriteLine(fibonacciGetter.GetValueByNumber(10, CacheType.RuntimeCaching));
            Console.WriteLine("Общее кол-во вызовов метода расчета: {0}", fibonacciGetter.ValCountCalc);

            Console.WriteLine();
            Console.WriteLine("Для завершения нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}
