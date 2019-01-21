using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal interface IAdapter : IDisposable
    {
        bool IsConnected { get; }

        Stream Connect();
    }
}
