using OneWay.M3U.Core;

namespace OneWay.M3U.AttributeReaders
{
    internal class PlaylistTypeAttributeReader : AttributeReader, IAttributeReader
    {
        public PlaylistTypeAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.PlaylistType, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader) =>
            fileInfo.PlaylistType = value;
    }
}
