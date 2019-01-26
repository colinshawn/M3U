using System;

namespace OneWay.M3U
{
    public class M3UKeyInfo
    {
        public string Method { get; set; }

        public Uri Uri { get; set; }

        public string IV { get; set; }

        public M3UKeyInfo()
        {
        }
    }
}
