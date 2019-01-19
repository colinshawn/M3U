using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace OneWay.M3U.UnitTest
{
    [TestClass]
    public class M3UFileUnitTest
    {
        const string FileName = "demo.m3u8";
        const string Url = "https://media-qncdn.ruguoapp.com/bfa35fe298f015b552bdf922fff846c31d568a75abf7e037861318cf288f8bff-5c411f300c62b0bf16f6523f.m3u8";

        [TestMethod]
        public void TestFile()
        {
            Configuration.Default.BaseUri = new Uri("https://media-qncdn.ruguoapp.com/", UriKind.Absolute);
            var fileInfo = M3UFile.FromFile(FileName);
            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        public void TestStream()
        {
            Configuration.Default.BaseUri = new Uri("https://media-qncdn.ruguoapp.com/", UriKind.Absolute);
            var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileInfo = M3UFile.FromStream(stream);
            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        public void TestUrl()
        {
            Configuration.Default.BaseUri = new Uri("https://media-qncdn.ruguoapp.com/", UriKind.Absolute);
            var fileInfo = M3UFile.FromUrl(Url);
            Assert.IsNotNull(fileInfo);
        }

        [TestMethod]
        public void TestText()
        {
            Configuration.Default.BaseUri = new Uri("https://media-qncdn.ruguoapp.com/", UriKind.Absolute);
            var text = @"#EXTM3U
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=1280000
            http://example.com/low.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=2560000
            http://example.com/mid.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=7680000
            http://example.com/hi.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=65000,CODECS=""mp4a.40.5""
            http://example.com/audio-only.m3u8
             ";
            var fileInfo = M3UFile.FromText(text);
            Assert.IsNotNull(fileInfo);
        }
    }
}
