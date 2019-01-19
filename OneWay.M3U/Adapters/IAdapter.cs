using System.IO;

namespace OneWay.M3U.Adapters
{
    internal interface IAdapter
    {
        Stream Access();
    }
}
