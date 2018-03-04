using System;

namespace Web
{
    public interface IProxy : IDisposable
    {
        string GetData(string url);
    }
}
