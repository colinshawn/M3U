using OneWay.M3U.Core;

namespace OneWay.M3U.AttributeReaders
{
    internal interface IAttributeReader
    {
        /// <returns>Indicates whether to terminate.</returns>
        bool Read(LineReader reader, M3UFileInfo fileInfo);
    }
}
