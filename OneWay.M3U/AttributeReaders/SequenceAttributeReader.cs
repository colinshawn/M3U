using OneWay.M3U.Core;
using OneWay.M3U.Utilities;

namespace OneWay.M3U.AttributeReaders
{
    internal class SequenceAttributeReader : AttributeReader, IAttributeReader
    {
        public SequenceAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.MediaSequence, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.MediaSequence = To.Value<int>(value);
    }
}
