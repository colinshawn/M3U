using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal class StreamAdapter : IAdapter
    {
        public Stream Source { get; private set; }

        public StreamAdapter(Stream source) =>
            this.Source = source ?? throw new ArgumentNullException(nameof(source));

        public Stream Access() => this.Source;
    }
}
