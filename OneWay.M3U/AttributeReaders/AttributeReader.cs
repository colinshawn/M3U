using OneWay.M3U.Core;
using OneWay.M3U.Utilities;

namespace OneWay.M3U.AttributeReaders
{
    internal abstract class AttributeReader : IAttributeReader
    {
        protected abstract bool CanRead(string key);

        protected virtual bool ShouldTerminate() => false;

        protected abstract void Write(M3UFileInfo fileInfo, string value, LineReader reader);

        public bool Read(LineReader reader, M3UFileInfo fileInfo)
        {
            var line = reader.Current?.Trim();
            if (string.IsNullOrEmpty(line))
                return false;
            if (M3UAttributes.TagIdentifier != line[0])
                return false;

            var kv = KV.Parse(line, M3UAttributes.TagSeparator).Value;
            if (!CanRead(kv.Key))
                return false;
            if (ShouldTerminate())
                return true;

            Write(fileInfo, kv.Value, reader);

            return false;
        }
    }
}
