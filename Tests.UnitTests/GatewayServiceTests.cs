using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockMarket.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StockMarket.Factories;
using Moq;
using AutoFixture;
using PriceMonitor;
using System.Configuration;

namespace Tests.UnitTests
{
    [TestClass]
    public class GatewayServiceTests
    {
        [TestMethod]
        public void InvalidateGateway_WhenInvalidateGateway_ShouldClearUrl()
        {
            // Arrange
            var serializationService = new SerializationService();
            var httpWebRequestFactory = new HttpWebRequestFactory();
            var gatewayService = new GatewayService(serializationService, httpWebRequestFactory);

            gatewayService.Url = "http://www.test.com";

            // Act
            gatewayService.InvalidateGateway();

            // Assert
            gatewayService.Url.Should().BeNull();
        }

        [TestMethod]
        public void LoadBestGateway_WhenValidatingGateway_ShouldSetTheUrl()
        {
            // Arrange
            var acoes = new string[] { "IBOV", "PETR4" };
            var urlTemplate = ConfigurationManager.AppSettings["UrlGatewayTemplate"].Replace("https://{0}", "");

            var expected = "response content";
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var acoesJsonReaderPriceBuilder = new Fixture().Build<AcoesJsonReaderPrice>();

            var mockedJsonResult = new AcoesJsonReaderPriceCollection()
            {
                Value = new List<AcoesJsonReaderPrice>()
            };

            foreach (var acao in acoes)
            {
                var newAcao = acoesJsonReaderPriceBuilder.With(x => x.S, acao).Create();
                newAcao.Ps.P = 10M;
                newAcao.Ps.OP = 10M;
                newAcao.Ps.CP = 10M;

                mockedJsonResult.Value.Add(newAcao);
            }

            var serializationService = new Mock<ISerializationService>();
            serializationService.Setup(x => x.DeserializeJson(It.IsAny<string>())).Returns(mockedJsonResult);
            var httpWebResponse = new Mock<IHttpWebResponse>();
            httpWebResponse.Setup(x => x.GetResponseStream()).Returns(responseStream);
            httpWebResponse.Setup(x => x.ContentType).Returns("");
            var httpWebRequest = new Mock<IHttpWebRequest>();
            httpWebRequest.Setup(x => x.GetResponse()).Returns(httpWebResponse.Object);
            var httpWebRequestFactory = new Mock<IHttpWebRequestFactory>();
            httpWebRequestFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(httpWebRequest.Object);

            // Act
            var gatewayService = new GatewayService(serializationService.Object, httpWebRequestFactory.Object);
            gatewayService.LoadBestGateway(acoes);

            // Assert
            gatewayService.Url.Should().Contain(urlTemplate);

            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Once);
            httpWebResponse.Verify(x => x.GetResponseStream(), Times.Once);
            httpWebResponse.Verify(x => x.ContentType, Times.Once);
            httpWebRequest.Verify(x => x.GetResponse(), Times.Once);
            httpWebRequestFactory.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
        }
    }
}
