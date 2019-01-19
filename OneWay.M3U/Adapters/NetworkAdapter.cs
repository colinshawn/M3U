using System;
using System.IO;
using System.Net;

namespace OneWay.M3U.Adapters
{
    internal class NetworkAdapter : IAdapter
    {
        public string Url { get; private set; }

        public NetworkAdapter(string url) =>
            this.Url = url ?? throw new ArgumentNullException(nameof(url));

        private static HttpWebRequest CreateHttp(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;
            request.UserAgent = Configuration.Default.UserAgent;
            request.Timeout = Configuration.Default.RequestTimeout;

            return request;
        }

        private static Stream GetResponseStream(WebRequest request)
        {
            var response = request.GetResponse();

            return response.GetResponseStream();
        }

        public Stream Access() => GetResponseStream(CreateHttp(this.Url));
    }
}
