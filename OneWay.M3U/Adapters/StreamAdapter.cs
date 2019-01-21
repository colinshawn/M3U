using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class StreamAdapter : Adapter, IAdapter
    {
        public new Stream Stream => base.Stream;

        public StreamAdapter(Stream stream) =>
            base.Stream = stream ?? throw new ArgumentNullException(nameof(stream));

        protected override Stream CreateStream() => Stream;
    }
}
