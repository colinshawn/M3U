using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class FileAdapter : Adapter, IAdapter
    {
        public FileInfo File { get; private set; }

        public FileAdapter(string fileName)
            : this(new FileInfo(fileName ?? throw new ArgumentNullException(nameof(fileName))))
        {
        }

        public FileAdapter(FileInfo file)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));

            if (!File.Exists)
                throw new FileNotFoundException("File not found.", File.FullName);
        }

        protected override Stream CreateStream() =>
            File.OpenRead();
    }
}
