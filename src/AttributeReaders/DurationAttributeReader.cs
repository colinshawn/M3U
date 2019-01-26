using OneWay.M3U.Core;
using OneWay.M3U.Utilities;

namespace OneWay.M3U.AttributeReaders
{
    internal class DurationAttributeReader : AttributeReader, IAttributeReader
    {
        public DurationAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.TargetDuration, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.TargetDuration = To.Value<int>(value);
    }
}
