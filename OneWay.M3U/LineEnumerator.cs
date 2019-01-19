using OneWay.M3U.Adapters;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OneWay.M3U
{
    internal sealed class LineEnumerator : IEnumerator<string>
    {
        private StreamReader _reader;

        public LineEnumerator(IAdapter adapter) =>
             this._reader = new StreamReader(adapter.Access());

        public string Current { get; private set; }

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
            this._reader.Dispose();
            this._reader = null;
        }

        public bool MoveNext()
        {
            var reader = this._reader;
            var eos = reader.EndOfStream;

            this.Current = eos ? default : reader.ReadLine();

            return !eos;
        }

        public void Reset()
        {
            var reader = this._reader;
            var source = reader.BaseStream;
            if (source.CanSeek)
                source.Seek(default, SeekOrigin.Begin);

            reader.DiscardBufferedData();
            this.Current = default;
        }
    }
}
