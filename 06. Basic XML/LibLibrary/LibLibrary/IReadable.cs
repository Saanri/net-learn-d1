using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibLibrary
{
    interface IReadable
    {
        string TakeRead(string readerName);
        string Return();
    }
}
