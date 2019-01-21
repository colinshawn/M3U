using OneWay.M3U.Adapters;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OneWay.M3U.Core
{
    internal sealed class LineReader : IEnumerator<string>
    {
        private readonly StreamReader reader;

        public string Current { get; private set; }

        object IEnumerator.Current => Current;

        private LineReader()
        {
        }

        public LineReader(IAdapter adapter) =>
             reader = new StreamReader(adapter.Connect());

        public void Dispose() => reader.Dispose();

        public bool MoveNext()
        {
            var reader = this.reader;
            var eos = reader.EndOfStream;

            Current = eos ? default : reader.ReadLine();

            return !eos;
        }

        public void Reset()
        {
            var reader = this.reader;
            var source = reader.BaseStream;
            if (source.CanSeek)
                source.Seek(default, SeekOrigin.Begin);

            reader.DiscardBufferedData();
            Current = default;
        }
    }
}
