using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class FileAdapter : IAdapter
    {
        public string Path { get; private set; }

        public FileAdapter(string path) =>
            this.Path = path ?? throw new ArgumentNullException(nameof(path));

        public Stream Access() =>
            File.Open(this.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }
}
