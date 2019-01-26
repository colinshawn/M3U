using System;

namespace OneWay.M3U
{
    public class M3UStreamInfo
    {
        public int? ProgramId { get; set; }

        public int? Bandwidth { get; set; }

        public string Codecs { get; set; }

        public string Resolution { get; set; }

        public Uri Uri { get; set; }

        public M3UStreamInfo()
        {
        }
    }
}
