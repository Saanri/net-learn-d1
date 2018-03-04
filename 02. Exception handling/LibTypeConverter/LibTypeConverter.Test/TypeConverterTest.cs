using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibTypeConverter.Test
{
	[TestClass]
	public class TypeConverterTest
	{
		private int i;
		private ErrorCode errorCode;

		[TestInitialize]
		public void Initialize()
		{
			i = 0;
		}

		[TestMethod]
		public void TypeConverterTestStrToInt_Ok()
		{
			bool b = TypeConverter.StrToInt("1", ref i, out errorCode);

			Assert.IsTrue(b);
			Assert.AreEqual(i, 1);
			Assert.AreEqual(errorCode, ErrorCode.Ok);
		}

		[TestMethod]
		public void TypeConverterTestStrToInt_FormatException()
		{
			bool b = TypeConverter.StrToInt("один", ref i, out errorCode);

			Assert.IsFalse(b);
			Assert.AreEqual(i, 0);
			Assert.AreEqual(errorCode, ErrorCode.FormatException);
		}

		[TestMethod]
		public void TypeConverterTestStrToInt_OverflowException()
		{
			bool b = TypeConverter.StrToInt("0123456787901234567890123456789", ref i, out errorCode);

			Assert.IsFalse(b);
			Assert.AreEqual(i, 0);
			Assert.AreEqual(errorCode, ErrorCode.OverflowException);
		}
	}
}
