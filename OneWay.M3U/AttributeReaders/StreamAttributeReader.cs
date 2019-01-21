using OneWay.M3U.Core;
using OneWay.M3U.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneWay.M3U.AttributeReaders
{
    internal class StreamAttributeReader : AttributeReader, IAttributeReader
    {
        public StreamAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.StreamInf, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader)
        {
            var attrs = value
                                .Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => KV.Parse(e, M3UAttributes.PairSeparator).Value);
            if (fileInfo.Streams == null && attrs.Any())
                fileInfo.Streams = new List<M3UStreamInfo>();

            var stream = new M3UStreamInfo();
            foreach (var attr in attrs)
            {
                switch (attr.Key)
                {
                    case M3UAttributes.StreamInfAttributes.Bandwidth:
                        stream.Bandwidth = To.Value<int>(attr.Value);
                        break;
                    case M3UAttributes.StreamInfAttributes.ProgramId:
                        stream.ProgramId = To.Value<int>(attr.Value);
                        break;
                    case M3UAttributes.StreamInfAttributes.Codecs:
                        stream.Codecs = attr.Value;
                        break;
                    case M3UAttributes.StreamInfAttributes.Resolution:
                        stream.Resolution = attr.Value;
                        break;
                }
            }

            if (!reader.MoveNext())
                throw new InvalidDataException("Invalid M3U file. Missing a stream URI.");

            var streamUri = new Uri(reader.Current, UriKind.RelativeOrAbsolute);
            if (!streamUri.IsAbsoluteUri && !streamUri.IsWellFormedOriginalString())
                throw new InvalidDataException("Invalid M3U file. Include a invalid stream URI.",
                    innerException: new UriFormatException(reader.Current));

            stream.Uri = true == Configuration.Default.BaseUri?.IsAbsoluteUri && !streamUri.IsAbsoluteUri ?
                new Uri(Configuration.Default.BaseUri, streamUri) : streamUri;

            fileInfo.Streams.Add(stream);
        }
    }
}
