using System;
using System.IO;

namespace OneWay.M3U.Adapters
{
    internal abstract class Adapter : IAdapter, IDisposable
    {
        protected Stream Stream { get; set; }

        public bool IsConnected { get; protected set; }

        protected abstract Stream CreateStream();

        public Stream Connect()
        {
            if (IsConnected)
                return Stream;

            Stream = CreateStream();
            if (Stream == null)
                throw new NullReferenceException($"{nameof(Stream)} is null.");

            IsConnected = true;

            return Stream;
        }

        public void Dispose()
        {
            IsConnected = false;
            Stream?.Dispose();
        }
    }
}
