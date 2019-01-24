Usage examples:

M3UFileInfo m3uFile;

// read from file
var file = new FileInfo("example.m3u");
using (var reader = new M3UFileReader(file))
    m3uFile = reader.Read();


// read from stream
var fs = File.OpenRead("example.m3u");
using (var reader = new M3UFileReader(fs))
    m3uFile = reader.Read();


// read from text
// set the base URI if only specified the name in M3U file
Configuration.Default.BaseUri = new Uri("http://example.com", UriKind.Absolute);
var text = @"#EXTM3U
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=1280000
low.m3u8
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=2560000
mid.m3u8
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=7680000
hi.m3u8
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=65000,CODECS=""mp4a.40.5""
audio-only.m3u8";
using (var reader = new M3UFileReader(text))
    m3uFile = reader.Read();

// you can get the following URIs:
// http://example.com/low.m3u8
// http://example.com/mid.m3u8
// http://example.com/hi.m3u8
// http://example.com/audio-only.m3u8


// read from URI:
// if the following URI contains the same text as above, I will use it as the base URI
var uri = new Uri("http://example.com/example.m3u");
using (var reader = new M3UFileReader(uri))
    m3uFile = reader.Read();