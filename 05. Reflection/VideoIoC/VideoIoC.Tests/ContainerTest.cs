using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using VideoLib;

namespace VideoIoC.Tests
{
	[TestClass]
	public class ContainerTest
	{
        private Container _container;

		[TestInitialize]
		public void SetUp()
		{
			Assembly assembly = Assembly.Load("VideoLib"); ;

            _container = new Container();
            _container.AddAssembly(assembly);
		}

		[TestMethod]
		public void ConteinerCreateVideoManager1ByTypeWithUsePublicProperiesTest()
		{
			var instance = _container.CreateInstance(typeof(VideoManager1));

			Assert.IsNotNull(instance);
			Assert.IsNotNull(((VideoManager1)instance).VideoProvider);
		}

		[TestMethod]
		public void ConteinerCreateVideoManager2ByTypeWithUseConstructorTest()
		{
			var instance = _container.CreateInstance(typeof(VideoManager2));

			Assert.IsNotNull(instance);
			Assert.IsNotNull(((VideoManager2)instance).VideoProvider);
		}

		[TestMethod]
		public void ConteinerCreateVideoManager1ByGenericWithUsePublicProperiesTest()
		{
			var instance = _container.CreateInstance<VideoManager1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.VideoProvider);
		}

		[TestMethod]
		public void ConteinerCreateVideoManager2ByGenericWithUseConstructorTest()
		{
			var instance = _container.CreateInstance<VideoManager2>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.VideoProvider);
		}
	}
}
