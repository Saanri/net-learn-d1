using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibLibrary
{
    public interface ILibrable
    {
        string GetShortInfo();
        string ToString();
        int GetPublicationYear();
    }
}
