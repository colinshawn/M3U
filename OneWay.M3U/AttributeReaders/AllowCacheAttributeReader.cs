using OneWay.M3U.Core;
using OneWay.M3U.Utilities;

namespace OneWay.M3U.AttributeReaders
{
    internal class AllowCacheAttributeReader : AttributeReader, IAttributeReader
    {
        public AllowCacheAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.AllowCache, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.AllowCache = To.Value<bool>(value);
    }
}
