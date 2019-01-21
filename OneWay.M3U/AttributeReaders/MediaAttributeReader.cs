using OneWay.M3U.Core;
using OneWay.M3U.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace OneWay.M3U.AttributeReaders
{
    internal class MediaAttributeReader : AttributeReader, IAttributeReader
    {
        public MediaAttributeReader()
        {
        }

        protected override bool CanRead(string key) => string.Equals(M3UAttributes.Inf, key);

        protected override void Write(M3UFileInfo fileInfo, string value, LineReader reader)
        {
            if (fileInfo.MediaFiles == null)
                fileInfo.MediaFiles = new List<M3UMediaInfo>();

            var media = new M3UMediaInfo();
            var attrs = value.Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (attrs.Length > 0)
            {
                media.Duration = To.Value<float>(attrs[0]);
                media.Title = attrs.Length > 1 ? attrs[1].Trim() : string.Empty;
            }

            if (!reader.MoveNext())
                throw new InvalidDataException("Invalid M3U file. Missing a media URI.");

            var mediaUri = new Uri(reader.Current, UriKind.RelativeOrAbsolute);
            if (!mediaUri.IsAbsoluteUri && !mediaUri.IsWellFormedOriginalString())
                throw new InvalidDataException("Invalid M3U file. Include a invalid media URI.",
                    innerException: new UriFormatException(reader.Current));

            media.Uri = true == Configuration.Default.BaseUri?.IsAbsoluteUri && !mediaUri.IsAbsoluteUri ?
                new Uri(Configuration.Default.BaseUri, mediaUri) : mediaUri;

            fileInfo.MediaFiles.Add(media);
        }
    }
}
