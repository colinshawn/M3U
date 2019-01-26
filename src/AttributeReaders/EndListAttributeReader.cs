using OneWay.M3U.Core;

namespace OneWay.M3U.AttributeReaders
{
    internal class EndListAttributeReader : AttributeReader, IAttributeReader
    {
        public EndListAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.EndList, key);

        protected override bool ShouldTerminate() => true;

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader)
        {
        }
    }
}
