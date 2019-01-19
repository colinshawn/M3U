using OneWay.M3U.Adapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneWay.M3U
{
    public static class M3UFile
    {
        private static IEnumerator<string> Join(IAdapter adapter) =>
            new LineEnumerator(adapter);

        internal static M3UFileInfo Process(IAdapter adapter)
        {
            var enumerator = Join(adapter);
            if (!enumerator.MoveNext())
            {
                enumerator.Dispose();
                throw new InvalidDataException("Invalid M3U file.");
            }

            var header = enumerator.Current.Trim();
            if (!string.Equals(header, M3UAttributes.Header))
            {
                enumerator.Dispose();
                throw new InvalidDataException("Missing M3U header.");
            }

            var file = new M3UFileInfo();
            while (enumerator.MoveNext())
            {
                var line = enumerator.Current.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (M3UAttributes.TagIdentifier == line[0])
                {
                    var kv = KV.Parse(line, M3UAttributes.TagSeparator).Value;
                    if (string.Equals(M3UAttributes.EndList, kv.Key))
                        break;

                    switch (kv.Key)
                    {
                        case M3UAttributes.Version:
                            file.Version = To.Value<int>(kv.Value);
                            break;
                        case M3UAttributes.TargetDuration:
                            file.TargetDuration = To.Value<int>(kv.Value);
                            break;
                        case M3UAttributes.MediaSequence:
                            file.MediaSequence = To.Value<int>(kv.Value);
                            break;
                        case M3UAttributes.AllowCache:
                            file.AllowCache = To.Value<bool>(kv.Value);
                            break;
                        case M3UAttributes.PlaylistType:
                            file.PlaylistType = kv.Value;
                            break;
                        case M3UAttributes.ProgramDateTime:
                            file.ProgramDateTime = To.Value<DateTime>(kv.Value);
                            break;
                        case M3UAttributes.Key:
                            var keyAttrs = kv.Value
                                .Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => KV.Parse(e, M3UAttributes.PairSeparator).Value);
                            if (file.Key == null && keyAttrs.Any())
                                file.Key = new M3UKeyInfo();

                            foreach (var attr in keyAttrs)
                            {
                                switch (attr.Key)
                                {
                                    case M3UAttributes.KeyAttributes.Method:
                                        file.Key.Method = attr.Value;
                                        break;
                                    case M3UAttributes.KeyAttributes.Uri:
                                        file.Key.Uri = Uri.TryCreate(attr.Value, UriKind.Absolute, out var ___) ? ___ : default;
                                        break;
                                    case M3UAttributes.KeyAttributes.IV:
                                        file.Key.IV = attr.Value;
                                        break;
                                }
                            }

                            break;
                        case M3UAttributes.StreamInf:
                            var streamAttrs = kv.Value
                                .Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => KV.Parse(e, M3UAttributes.PairSeparator).Value);
                            if (file.Streams == null && streamAttrs.Any())
                                file.Streams = new List<M3UStreamInfo>();

                            var stream = new M3UStreamInfo();
                            foreach (var attr in streamAttrs)
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

                            if (!enumerator.MoveNext())
                            {
                                enumerator.Dispose();
                                throw new InvalidDataException("Invalid M3U file. Missing a stream URI.");
                            }

                            var streamUri = new Uri(enumerator.Current, UriKind.RelativeOrAbsolute);
                            if (!streamUri.IsAbsoluteUri && !streamUri.IsWellFormedOriginalString())
                            {
                                enumerator.Dispose();
                                throw new InvalidDataException("Invalid M3U file. Include a invalid stream URI.",
                                    innerException: new UriFormatException(enumerator.Current));
                            }

                            stream.Uri = true == Configuration.Default.BaseUri?.IsAbsoluteUri && !streamUri.IsAbsoluteUri ?
                                new Uri(Configuration.Default.BaseUri, streamUri) : streamUri;

                            file.Streams.Add(stream);

                            break;
                        case M3UAttributes.Inf:
                            if (file.MediaFiles == null)
                                file.MediaFiles = new List<M3UMediaInfo>();

                            var media = new M3UMediaInfo();
                            var infoAttrs = kv.Value.Split(new[] { M3UAttributes.AttributeSeparator }, StringSplitOptions.RemoveEmptyEntries);
                            if (infoAttrs.Length > 0)
                            {
                                media.Duration = To.Value<float>(infoAttrs[0]);
                                media.Title = infoAttrs.Length > 1 ? infoAttrs[1].Trim() : string.Empty;
                            }

                            if (!enumerator.MoveNext())
                            {
                                enumerator.Dispose();
                                throw new InvalidDataException("Invalid M3U file. Missing a media URI.");
                            }

                            var mediaUri = new Uri(enumerator.Current, UriKind.RelativeOrAbsolute);
                            if (!mediaUri.IsAbsoluteUri && !mediaUri.IsWellFormedOriginalString())
                            {
                                enumerator.Dispose();
                                throw new InvalidDataException("Invalid M3U file. Include a invalid media URI.",
                                    innerException: new UriFormatException(enumerator.Current));
                            }

                            media.Uri = true == Configuration.Default.BaseUri?.IsAbsoluteUri && !mediaUri.IsAbsoluteUri ?
                                new Uri(Configuration.Default.BaseUri, mediaUri) : mediaUri;

                            file.MediaFiles.Add(media);

                            break;
                    }
                }
            }

            enumerator.Dispose();

            return file;
        }

        public static M3UFileInfo FromFile(string path) =>
            Process(new FileAdapter(path));

        public static M3UFileInfo FromUrl(string url) =>
            Process(new NetworkAdapter(url));

        public static M3UFileInfo FromStream(Stream source) =>
            Process(new StreamAdapter(source));

        public static M3UFileInfo FromText(string content) =>
            Process(new TextAdapter(content));
    }
}
