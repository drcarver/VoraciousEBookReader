using System.Net;
using System.Net.Http;
using System.Text;

using Castle.Core.Logging;

using Moq;
using Moq.Protected;

using RichardSzalay.MockHttp;

using static System.Net.Mime.MediaTypeNames;

namespace VoraciousEBookReader.Tests;

[TestClass]
public partial class GutenbergCatalogTests
{
    // The catalog url
    private const string CATALOGURL = "https://www.gutenberg.org/cache/epub/feeds/pg_catalog.csv";

    List<string> data =
    ["Text#,Type,Issued,Title,Language,Authors,Subjects,LoCC,Bookshelves",
        "1,Text,1971-12-01,The Declaration of Independence of the United States of America, en,\"Jefferson, Thomas, 1743-1826\",\"United States -- History -- Revolution, 1775-1783 -- Sources; United States. Declaration of Independence\",E201; JK,Politics; American Revolutionary War; United States Law",
        "2,Text,1972-12-01,\"The United States Bill of Rights" +
            "\"The Ten Original Amendments to the Constitution of the United States\",en,United States,Civil rights -- United States -- Sources; United States. Constitution. 1st-10th Amendments,JK; KF,Politics; American Revolutionary War; United States Law",
        "3,Text,1973-11-01,John F.Kennedy's Inaugural Address,en,\"Kennedy, John F. (John Fitzgerald), 1917-1963\",United States -- Foreign relations -- 1961-1963; Presidents -- United States -- Inaugural addresses,E838,",
    ];

    [TestMethod]
    public void GutenbergCatalogServiceTest()
    {
        //var mockLoggerFactory = new Mock<ILoggerFactory>();
        //var mockFactory = new Mock<IHttpClientFactory>();
        //var mockHandler = new Mock<HttpMessageHandler>();
        //var mockLogger = new Mock<ILogger>();

        //mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        //IHttpClientFactory factory = mockFactory.Object;

        //var clientHandlerMock = new Mock<DelegatingHandler>();
        //clientHandlerMock.Protected()
        //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        //    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
        //    .Verifiable();
        //clientHandlerMock.As<IDisposable>().Setup(s => s.Dispose());

        //var httpClient = new HttpClient(clientHandlerMock.Object);

        //var clientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
        //clientFactoryMock.Setup(cf => cf.CreateClient()).Returns(httpClient).Verifiable();

        //clientFactoryMock.Verify(cf => cf.CreateClient());
        //clientHandlerMock.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        //// get the mock http object
        //string fullText = string.Join(Environment.NewLine, data);
        //var content = new MemoryStream(Encoding.UTF8.GetBytes(fullText));
        //content.Position = 0;
        //var mockHttp = new MockHttpMessageHandler();
        //mockHttp.When(CATALOGURL)
        //    .Respond(HttpStatusCode.OK, "type/csv", content);
    }
}

