using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class TextAdapter : IAdapter
    {
        public string Content { get; private set; }

        public TextAdapter(string content) =>
            this.Content = content ?? throw new ArgumentNullException(nameof(content));

        public Stream Access() =>
            new MemoryStream(Configuration.Default.Encoding.GetBytes(this.Content), writable: false);
    }
}
