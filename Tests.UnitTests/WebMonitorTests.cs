using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceMonitor;
using StockMarket.Services;
using System;
using System.Collections.Generic;

namespace Tests.UnitTests
{
    [TestClass]
    public class WebMonitorTests
    {
        [TestMethod]
        public void Run_WhenSuccessOnWebRequest_ShouldProcessResults()
        {
            var acoes = new string[] { "IBOV", "PETR4" };

            var builder = new Fixture().Build<AcoesJsonReaderPrice>();

            var mockedJsonResult = new AcoesJsonReaderPriceCollection()
            {
                Value = new List<AcoesJsonReaderPrice>()
            };

            foreach (var acao in acoes)
                mockedJsonResult.Value.Add(builder.With(x => x.S, acao).Create());

            //Mock
            var gatewayServiceMock = new Mock<IGatewayService>();
            gatewayServiceMock.Setup(x => x.LoadBestGateway(It.IsAny<string[]>()));
            gatewayServiceMock.Setup(x => x.RequestJson(It.IsAny<string>())).Returns("{}");
            gatewayServiceMock.Setup(x => x.Url).Returns("http://www.test.com/");
            var serializationService = new Mock<ISerializationService>();
            serializationService.Setup(x => x.DeserializeJson(It.IsAny<string>())).Returns(mockedJsonResult);
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(true);

            var webMonitor = new WebMonitor(acoes, gatewayServiceMock.Object, serializationService.Object, configsService.Object);

            Action action = () => webMonitor.Run((_) => { }, () => { }, () => { }, (_) => { }, (_, _) => { });
            action.Should().NotThrow();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Once);
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Once);
            gatewayServiceMock.Verify(x => x.Url, Times.Exactly(3));
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Once);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }
    }
}
