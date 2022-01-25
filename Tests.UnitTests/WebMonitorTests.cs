using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceMonitor;
using StockMarket;
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
            // Arrange
            var acoes = new string[] { "IBOV", "PETR4" };

            var mainCallback = false;
            var marcketOpen = false;
            var marcketClosed = false;
            var callbackError = false;
            var callbackNotification = false;

            var acoesJsonReaderPriceBuilder = new Fixture().Build<AcoesJsonReaderPrice>();

            var mockedJsonResult = new AcoesJsonReaderPriceCollection()
            {
                Value = new List<AcoesJsonReaderPrice>()
            };

            foreach (var acao in acoes)
            {
                var newAcao = acoesJsonReaderPriceBuilder.With(x => x.S, acao).Create();
                newAcao.Ps.MnP = 10M;

                mockedJsonResult.Value.Add(newAcao);
            }

            var acoeBuilder = new Fixture().Build<Acao>();

            var mockedAcoesCollection = new List<AcoesCollection>();
            foreach (var acao in acoes)
            {
                var newAcao = new AcoesCollection()
                {
                    Name = acao,
                    Acoes = new List<Acao>() { acoeBuilder.With(x => x.MinimunPrice, 50M).Create() }
                };

                mockedAcoesCollection.Add(newAcao);
            }

            var gatewayServiceMock = new Mock<IGatewayService>();
            gatewayServiceMock.Setup(x => x.LoadBestGateway(It.IsAny<string[]>()));
            gatewayServiceMock.Setup(x => x.RequestJson(It.IsAny<string>())).Returns("{}");
            gatewayServiceMock.Setup(x => x.Url).Returns("http://www.test.com/");
            var serializationService = new Mock<ISerializationService>();
            serializationService.Setup(x => x.DeserializeJson(It.IsAny<string>())).Returns(mockedJsonResult);
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(true);
            var persistenceService = new Mock<IPersistenceService>();
            persistenceService.Setup(x => x.LoadFromFiles(It.IsAny<string[]>())).Returns(mockedAcoesCollection);

            var webMonitor = new WebMonitor(acoes, gatewayServiceMock.Object, serializationService.Object, configsService.Object, persistenceService.Object);

            // Act
            Action action = () => webMonitor.Run(
                (_) => { mainCallback = true; },
                () => { marcketOpen = true; },
                () => { marcketClosed = true; },
                (_) => { callbackError = true; },
                (_, _) => { callbackNotification = true; });

            // Assert
            action.Should().NotThrow();

            mainCallback.Should().BeTrue();
            marcketOpen.Should().BeTrue();
            marcketClosed.Should().BeFalse();
            callbackError.Should().BeFalse();
            callbackNotification.Should().BeTrue();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Once);
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Once);
            gatewayServiceMock.Verify(x => x.InvalidateGateway(), Times.Never);
            gatewayServiceMock.Verify(x => x.Url, Times.Exactly(3));
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Once);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }

        [TestMethod]
        public void Run_WhenMarcketClosed_ShouldSkip()
        {
            // Arrange
            var mainCallback = false;
            var marcketOpen = false;
            var marcketClosed = false;
            var callbackError = false;
            var callbackNotification = false;

            var gatewayServiceMock = new Mock<IGatewayService>();
            var serializationService = new Mock<ISerializationService>();
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(false);

            var webMonitor = new WebMonitor(null, gatewayServiceMock.Object, null, configsService.Object, null);

            // Act
            Action action = () => webMonitor.Run(
                (_) => { mainCallback = true; },
                () => { marcketOpen = true; },
                () => { marcketClosed = true; },
                (_) => { callbackError = true; },
                (_, _) => { callbackNotification = true; });

            // Assert
            action.Should().NotThrow();

            mainCallback.Should().BeFalse();
            marcketOpen.Should().BeFalse();
            marcketClosed.Should().BeTrue();
            callbackError.Should().BeFalse();
            callbackNotification.Should().BeFalse();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Once);
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Never);
            gatewayServiceMock.Verify(x => x.InvalidateGateway(), Times.Never);
            gatewayServiceMock.Verify(x => x.Url, Times.Never);
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Never);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }

        [TestMethod]
        public void Run_WhenUrlIsEmpty_ShouldRetestGateway()
        {
            // Arrange
            var mainCallback = false;
            var marcketOpen = false;
            var marcketClosed = false;
            var callbackError = false;
            var callbackNotification = false;

            var gatewayServiceMock = new Mock<IGatewayService>();
            var serializationService = new Mock<ISerializationService>();
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(true);

            var webMonitor = new WebMonitor(null, gatewayServiceMock.Object, null, configsService.Object, null);

            // Act
            Action action = () => webMonitor.Run(
                (_) => { mainCallback = true; },
                () => { marcketOpen = true; },
                () => { marcketClosed = true; },
                (_) => { callbackError = true; },
                (_, _) => { callbackNotification = true; });

            // Assert
            action.Should().NotThrow();

            mainCallback.Should().BeFalse();
            marcketOpen.Should().BeFalse();
            marcketClosed.Should().BeFalse();
            callbackError.Should().BeTrue();
            callbackNotification.Should().BeFalse();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Exactly(2));
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Never);
            gatewayServiceMock.Verify(x => x.InvalidateGateway(), Times.Never);
            gatewayServiceMock.Verify(x => x.Url, Times.Once);
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Never);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }

        [TestMethod]
        public void Run_WhenJsonIsEmpty_ShouldInvalidateGateway()
        {
            // Arrange
            var mainCallback = false;
            var marcketOpen = false;
            var marcketClosed = false;
            var callbackError = false;
            var callbackNotification = false;

            var gatewayServiceMock = new Mock<IGatewayService>();
            gatewayServiceMock.Setup(x => x.RequestJson(It.IsAny<string>())).Returns(string.Empty);
            gatewayServiceMock.Setup(x => x.Url).Returns("http://www.test.com/");
            var serializationService = new Mock<ISerializationService>();
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(true);

            var webMonitor = new WebMonitor(null, gatewayServiceMock.Object, null, configsService.Object, null);

            // Act
            Action action = () => webMonitor.Run(
                (_) => { mainCallback = true; },
                () => { marcketOpen = true; },
                () => { marcketClosed = true; },
                (_) => { callbackError = true; },
                (_, _) => { callbackNotification = true; });

            // Assert
            action.Should().NotThrow();

            mainCallback.Should().BeFalse();
            marcketOpen.Should().BeFalse();
            marcketClosed.Should().BeFalse();
            callbackError.Should().BeTrue();
            callbackNotification.Should().BeFalse();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Once);
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Once);
            gatewayServiceMock.Verify(x => x.InvalidateGateway(), Times.Once);
            gatewayServiceMock.Verify(x => x.Url, Times.Exactly(3));
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Never);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }

        [TestMethod]
        public void Run_WhenJsonHasNotAllTicketsOrZeroPrice_ShouldReportError()
        {
            // Arrange
            var acoes = new string[] { "IBOV", "PETR4", "ABC1" };

            var mainCallback = false;
            var marcketOpen = false;
            var marcketClosed = false;
            var callbackError = false;
            var callbackNotification = false;

            var acoesJsonReaderPriceBuilder = new Fixture().Build<AcoesJsonReaderPrice>();

            var acao1 = acoesJsonReaderPriceBuilder.With(x => x.S, acoes[0]).Create();
            var acao2 = acoesJsonReaderPriceBuilder.With(x => x.S, acoes[1]).Create();
            acao2.Ps.P = 0;
            acao2.Ps.MnP = 0;
            var mockedJsonResult = new AcoesJsonReaderPriceCollection()
            {
                Value = new List<AcoesJsonReaderPrice>()
                {
                    acao1,
                    acao2
                }
            };

            var gatewayServiceMock = new Mock<IGatewayService>();
            gatewayServiceMock.Setup(x => x.RequestJson(It.IsAny<string>())).Returns("{}");
            gatewayServiceMock.Setup(x => x.Url).Returns("http://www.test.com/");
            var serializationService = new Mock<ISerializationService>();
            serializationService.Setup(x => x.DeserializeJson(It.IsAny<string>())).Returns(mockedJsonResult);
            var configsService = new Mock<IConfigsService>();
            configsService.Setup(x => x.IsMarketOpened).Returns(true);
            var persistenceService = new Mock<IPersistenceService>();
            persistenceService.Setup(x => x.LoadFromFiles(It.IsAny<string[]>())).Returns(new List<AcoesCollection>());

            var webMonitor = new WebMonitor(acoes, gatewayServiceMock.Object, serializationService.Object, configsService.Object, persistenceService.Object);

            // Act
            Action action = () => webMonitor.Run(
                (_) => { mainCallback = true; },
                () => { marcketOpen = true; },
                () => { marcketClosed = true; },
                (_) => { callbackError = true; },
                (_, _) => { callbackNotification = true; });

            // Assert
            action.Should().NotThrow();

            mainCallback.Should().BeTrue();
            marcketOpen.Should().BeFalse();
            marcketClosed.Should().BeFalse();
            callbackError.Should().BeTrue();
            callbackNotification.Should().BeFalse();

            gatewayServiceMock.Verify(x => x.LoadBestGateway(It.IsAny<string[]>()), Times.Once);
            gatewayServiceMock.Verify(x => x.RequestJson(It.IsAny<string>()), Times.Once);
            gatewayServiceMock.Verify(x => x.InvalidateGateway(), Times.Once);
            gatewayServiceMock.Verify(x => x.Url, Times.Exactly(3));
            serializationService.Verify(x => x.DeserializeJson(It.IsAny<string>()), Times.Once);
            configsService.Verify(x => x.IsMarketOpened, Times.Once);
        }
    }
}
