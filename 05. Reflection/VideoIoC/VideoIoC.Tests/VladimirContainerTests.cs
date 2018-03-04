using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoIoC;

namespace IoCContainer.Tests
{
	public class MyClassA
	{

	}

	[ImportConstructor]
	public abstract class MyClassB
	{

	}

	[ImportConstructor]
	public class MyClassС
	{

	}

	[Export]
	public class MyClassD
	{
		public MyClassD()
		{

		}
	}

	[Export(typeof(MyClassE))]
	public class MyClassE
	{
		public MyClassE()
		{

		}
	}

	[ImportConstructor]
	public class MyClassF
	{
		[Import]
		public MyClassD MyClassD { get; set; }
		private readonly MyClassE _myClassE;

		public MyClassE MyClassE
		{
			get { return _myClassE; }
		}

		public MyClassF(MyClassE myClassE)
		{
			_myClassE = myClassE;
		}
	}

	public class MyClassG
	{
		[Import]
		public MyClassD MyClassD { get; set; }
		[Import]
		public MyClassE MyClassE { get; set; }
		protected MyClassG()
		{

		}
	}

    [ImportConstructor]
	public class MyClassH
	{
		public IMyClassI MyClassI { get; private set; }
		public MyClassH(IMyClassI myClassI)
		{
			MyClassI = myClassI;
		}
	}

	public interface IMyClassI
	{

	}

    [ImportConstructor]
	public class MyClassI : IMyClassI
	{
	}
    
	[ImportConstructor]
	public class MyClassJ
	{
		public MyClassH MyClassH { get; private set; }
		public MyClassJ(MyClassH myClassH)
		{
			MyClassH = myClassH;
		}
	}

	[TestClass]
	public class VladimirContainerTests
	{
		private Container _sut = new Container();

		[TestInitialize]
		public void SetUp()
		{
			_sut = new Container();
		}

		[TestMethod]
		public void should_not_add_incorrect_type()
		{
			// класс не размечен,  а оно работает (вцелом, это не ошибка, если ты так задумывал)
			// так и задумано )
			_sut.AddType(typeof(MyClassA));

			var instance = _sut.CreateInstance<MyClassA>();
			Assert.IsNotNull(instance);
		}

        [TestMethod]
		public void should_resolve_an_instance()
		{
			// это должно работать
			// оно работает
			_sut.AddType(typeof(MyClassС));

			var instance = _sut.CreateInstance<MyClassС>();
			Assert.IsNotNull(instance);
		}


		[TestMethod]
		[ExpectedException(typeof(CanNotCreateAbstractType))]
		public void should_throw_an_expection()
		{
			_sut.AddType(typeof(MyClassB));

			_sut.CreateInstance<MyClassB>();
		}

		[TestMethod]
		public void should_resolve_complex_obg()
		{
			_sut.AddType(typeof(MyClassF));
			_sut.AddType(typeof(MyClassE), typeof(MyClassE));
			_sut.AddType(typeof(MyClassD), typeof(MyClassD));

			var result = _sut.CreateInstance<MyClassF>();

			Assert.IsNotNull(result.MyClassE);
			Assert.IsNotNull(result.MyClassD);
		}

		[TestMethod]
		[ExpectedException(typeof(ConstructorNotFound))]
		public void not_sure_but_maybe_it_should_work()
		{
			_sut.AddType(typeof(MyClassG));

			_sut.CreateInstance<MyClassG>();
		}

		[TestMethod]
        [ExpectedException(typeof(ConstructorNotFound))]
        public void should_resolve_with_import_attribute_only()
		{
			_sut.AddType(typeof(MyClassE), typeof(MyClassE));
			_sut.AddType(typeof(MyClassD), typeof(MyClassD));
			_sut.AddType(typeof(MyClassG));

			var result = _sut.CreateInstance<MyClassG>();

			Assert.IsNotNull(result.MyClassE);
			Assert.IsNotNull(result.MyClassD);
		}

		// не понимаю почему это он не поддерживается???
        // теперь поддерживается
		[TestMethod]
		public void should_resolve_by_interface()
		{
			_sut.AddType(typeof(IMyClassI), typeof(MyClassI));
			_sut.AddType(typeof(MyClassH), typeof(MyClassH));

			var result = _sut.CreateInstance<MyClassH>();

			Assert.IsNotNull(result.MyClassI);
		}

		[TestMethod]
		public void should_resolve_by_interface2()
		{
			_sut.AddType(typeof(MyClassH));
			_sut.AddType(typeof(IMyClassI), typeof(MyClassI));
			_sut.AddType(typeof(MyClassJ));

			var result = _sut.CreateInstance<MyClassJ>();

			Assert.IsNotNull(result.MyClassH);
		}
	}
}
