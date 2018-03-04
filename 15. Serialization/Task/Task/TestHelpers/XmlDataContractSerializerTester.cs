using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Task.TestHelpers
{
	public class XmlDataContractSerializerTester<T> : SerializationTester<T, XmlObjectSerializer>
	{
		public XmlDataContractSerializerTester(
			XmlObjectSerializer serializer, bool showResult = false) : base(serializer, showResult)
		{ }

		internal override T Deserialization(MemoryStream stream)
		{
			return (T)serializer.ReadObject(stream);
		}

		internal override void Serialization(T data, MemoryStream stream)
		{
			serializer.WriteObject(stream, data);
		}
    }

}
