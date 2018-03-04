using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FibonacciNumbers;

namespace FibonacciNumbersTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Fibonacci _fibonacciGetter = new Fibonacci();

        [TestMethod]
        public void GetFibonacciValueByNumberWithRedisCachingTest()
        {
            Assert.AreEqual(_fibonacciGetter.GetValueByNumber(12, CacheType.RedisCaching), 144);
            Assert.AreEqual(_fibonacciGetter.GetValueByNumber(12, CacheType.RedisCaching), 144);
            Assert.AreEqual(_fibonacciGetter.ValCountCalc, 1);
        }

        [TestMethod]
        public void GetFibonacciValueByNumberWithRuntimeCachingTest()
        {
            Assert.AreEqual(_fibonacciGetter.GetValueByNumber(10, CacheType.RuntimeCaching), 55);
            Assert.AreEqual(_fibonacciGetter.GetValueByNumber(10, CacheType.RuntimeCaching), 55);
            Assert.AreEqual(_fibonacciGetter.ValCountCalc, 1);
        }
    }
}
