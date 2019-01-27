using OneWay.M3U.Adapters;
using OneWay.M3U.AttributeReaders;
using OneWay.M3U.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneWay.M3U
{
    public class M3UFileReader : IDisposable
    {
        private M3UFileInfo cache;

        private readonly IAdapter adapter;

        private readonly IReadOnlyList<IAttributeReader> attributeReaders;

        private M3UFileReader()
        {
            attributeReaders = LoadAttributeReaders();
        }

        public M3UFileReader(FileInfo file) : this() =>
            adapter = new FileAdapter(file);

        public M3UFileReader(string text) : this() =>
            adapter = new TextAdapter(text);

        public M3UFileReader(Stream stream) : this() =>
            adapter = new StreamAdapter(stream);

        public M3UFileReader(Uri uri) : this() =>
            adapter = new NetworkAdapter(uri);

        private static IReadOnlyList<IAttributeReader> LoadAttributeReaders()
        {
            var tReader = typeof(IAttributeReader);
            var definedTypes = tReader.Assembly.DefinedTypes;
            return definedTypes
                        .Where(e => tReader.IsAssignableFrom(e) && e.IsClass && !e.IsAbstract)
                        .Select(Activator.CreateInstance)
                        .Cast<IAttributeReader>()
                        .ToList();
        }

        public void Dispose() => adapter?.Dispose();

        public M3UFileInfo Read()
        {
            if (cache != null)
                return cache;

            using (var reader = new LineReader(adapter))
            {
                if (!reader.MoveNext())
                    throw new InvalidDataException("Invalid M3U file.");

                var header = reader.Current.Trim();
                if (!string.Equals(header, M3UAttributes.Header))
                    throw new InvalidDataException("Missing M3U header.");

                var fileInfo = new M3UFileInfo();
                while (reader.MoveNext())
                {
                    var shouldTerminate = false;
                    foreach (var attrReader in attributeReaders)
                    {
                        shouldTerminate = attrReader.Read(reader, fileInfo);
                        if (shouldTerminate)
                            break;
                    }

                    if (shouldTerminate)
                        break;
                }

                return cache = fileInfo;
            }
        }
    }
}
