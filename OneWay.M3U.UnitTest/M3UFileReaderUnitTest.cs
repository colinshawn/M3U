using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace OneWay.M3U.UnitTest
{
    [TestClass]
    public class M3UFileReaderUnitTest
    {
        const string FileName = "demo.m3u8";
        const string Url = "https://media-qncdn.ruguoapp.com/bfa35fe298f015b552bdf922fff846c31d568a75abf7e037861318cf288f8bff-5c411f300c62b0bf16f6523f.m3u8";

        public M3UFileReaderUnitTest()
        {
            //Configuration.Default.BaseUri = new Uri("https://media-qncdn.ruguoapp.com/", UriKind.Absolute);
        }

        [TestMethod]
        public void TestFile()
        {
            M3UFileInfo m3uFile;
            var file = new FileInfo(FileName);
            using (var reader = new M3UFileReader(file))
                m3uFile = reader.Read();

            Assert.IsNotNull(m3uFile);
            Assert.IsNotNull(m3uFile.MediaFiles);
            Assert.IsTrue(m3uFile.MediaFiles.Count == 10);
        }

        [TestMethod]
        public void TestStream()
        {
            M3UFileInfo m3uFile;
            var fs = File.OpenRead(FileName);
            using (var reader = new M3UFileReader(fs))
                m3uFile = reader.Read();

            Assert.IsNotNull(m3uFile);
            Assert.IsNotNull(m3uFile.MediaFiles);
            Assert.IsTrue(m3uFile.MediaFiles.Count == 10);
        }

        [TestMethod]
        public void TestUri()
        {
            M3UFileInfo m3uFile;
            var uri = new Uri(Url);
            using (var reader = new M3UFileReader(uri))
                m3uFile = reader.Read();
            
            Assert.IsNotNull(m3uFile);
            Assert.IsNotNull(m3uFile.MediaFiles);
            Assert.IsTrue(m3uFile.MediaFiles.Count > 0);
        }

        [TestMethod]
        public void TestText()
        {
            M3UFileInfo m3uFile;
            Configuration.Default.BaseUri = new Uri("http://example.com", UriKind.Absolute);
            var text = @"#EXTM3U
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=1280000
            low.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=2560000
            http://example.com/mid.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=7680000
            hi.m3u8
            #EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=65000,CODECS=""mp4a.40.5""
            audio-only.m3u8
             ";
            using (var reader = new M3UFileReader(text))
                m3uFile = reader.Read();

            Assert.IsNotNull(m3uFile);
            Assert.IsNotNull(m3uFile.Streams);
            Assert.IsTrue(m3uFile.Streams.Count == 4);
        }
    }
}
