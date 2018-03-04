using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIoC
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImportAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ImportConstructorAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        public Type AttributeType { get; private set; }

        public ExportAttribute()
        { }

        public ExportAttribute(Type type)
        {
            AttributeType = type;
        }
    }
}
