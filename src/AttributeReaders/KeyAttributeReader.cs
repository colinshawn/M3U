using OneWay.M3U.Core;
using OneWay.M3U.Utilities;
using System;
using System.Linq;

namespace OneWay.M3U.AttributeReaders
{
    internal class KeyAttributeReader : AttributeReader, IAttributeReader
    {
        public KeyAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.Key, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader)
        {
            var attrs = value
                                .Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => KV.Parse(e, M3UAttributes.PairSeparator).Value);
            if (fileInfo.Key == null && attrs.Any())
                fileInfo.Key = new M3UKeyInfo();

            foreach (var attr in attrs)
            {
                switch (attr.Key)
                {
                    case M3UAttributes.KeyAttributes.Uri:
                        fileInfo.Key.Uri = Uri.TryCreate(attr.Value, UriKind.Absolute, out var uri) ? uri : default;
                        break;
                    case M3UAttributes.KeyAttributes.IV:
                        fileInfo.Key.IV = attr.Value;
                        break;
                    case M3UAttributes.KeyAttributes.Method:
                        fileInfo.Key.Method = attr.Value;
                        break;
                }
            }
        }
    }
}
