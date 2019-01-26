using System;
using System.Collections.Generic;

namespace OneWay.M3U
{
    /// <summary>
    ///  See https://tools.ietf.org/html/rfc8216
    /// </summary>
    public class M3UFileInfo
    {
        public int? Version { get; set; }

        public int? MediaSequence { get; set; }

        public int? TargetDuration { get; set; }

        public bool? AllowCache { get; set; }
        
        public string PlaylistType { get; set; }

        public DateTime? ProgramDateTime { get; set; }

        public M3UKeyInfo Key { get; set; }

        public IList<M3UStreamInfo> Streams { get; set; }

        public IList<M3UMediaInfo> MediaFiles { get; set; }

        public M3UFileInfo()
        {
        }
    }
}
