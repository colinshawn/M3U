using OneWay.M3U.Core;
using OneWay.M3U.Utilities;

namespace OneWay.M3U.AttributeReaders
{
    internal class VersionAttributeReader : AttributeReader, IAttributeReader
    {
        public VersionAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.Version, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.Version = To.Value<int>(value);
    }
}
