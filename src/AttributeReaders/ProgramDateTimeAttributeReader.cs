using OneWay.M3U.Core;
using OneWay.M3U.Utilities;
using System;

namespace OneWay.M3U.AttributeReaders
{
    internal class ProgramDateTimeAttributeReader : AttributeReader, IAttributeReader
    {
        public ProgramDateTimeAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.ProgramDateTime, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.ProgramDateTime = To.Value<DateTime>(value);
    }
}
