using System.IO;
using System.Text;
using System.Xml;
using PodcastUtilities.Common.Configuration;

namespace PodcastUtilities.Common.Tests.Configuration.PodcastInfoTests.Serialisation.Write
{
    public abstract class WhenWritingAPodcastInfo
        : WhenTestingBehaviour
    {
        protected IReadOnlyControlFile _controlFile;

        protected PodcastInfo _podcastInfo;
        protected MemoryStream _memoryStream;
        protected XmlWriter _xmlWriter;
        protected StreamReader _textReader;

        protected override void GivenThat()
        {
            base.GivenThat();

            _controlFile = TestControlFileFactory.CreateControlFile();
            _podcastInfo = new PodcastInfo(_controlFile);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = false;
            settings.Encoding = Encoding.UTF8;

            _memoryStream = new MemoryStream();
            _xmlWriter = XmlWriter.Create(_memoryStream, settings);
        }

        protected override void When()
        {
            _xmlWriter.Flush();
            _memoryStream.Position = 0;
            _textReader = new StreamReader(_memoryStream);
        }
    }
}