using System;
using System.IO;
using System.Net;

namespace OneWay.M3U.Adapters
{
    internal class NetworkAdapter : Adapter, IAdapter
    {
        public Uri Uri { get; private set; }

        public NetworkAdapter(string uriString)
            : this(new Uri(uriString ?? throw new ArgumentNullException(nameof(uriString)), UriKind.Absolute))
        {
        }

        public NetworkAdapter(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));

            if (!Uri.IsAbsoluteUri)
                throw new UriFormatException("Invalid URI string. Required an absolute URI.");
        }

        protected override Stream CreateStream()
        {
            var request = WebRequest.CreateHttp(Uri);
            request.Method = WebRequestMethods.Http.Get;
            request.UserAgent = Configuration.Default.UserAgent;
            request.Timeout = Configuration.Default.RequestTimeout;

            var response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
