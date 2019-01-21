using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class TextAdapter : Adapter, IAdapter
    {
        public string Text { get; private set; }

        public TextAdapter(string text) =>
            Text = text ?? throw new ArgumentNullException(nameof(text));

        protected override Stream CreateStream() =>
            new MemoryStream(Configuration.Default.Encoding.GetBytes(Text), writable: false);
    }
}
