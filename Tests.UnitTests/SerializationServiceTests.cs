using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PriceMonitor;
using StockMarket.Services;
using System.Collections.Generic;
using System.IO;

namespace Tests.UnitTests
{
    [TestClass]
    public class SerializationServiceTests
    {
        [TestMethod]
        public void DeserializeJson_WhenReceiveAJsonString_ShouldDeserializeToCollection()
        {
            // Arrange
            var acoes = new string[] { "IBOV", "PETR4" };

            var acoesJsonReaderPriceBuilder = new Fixture().Build<AcoesJsonReaderPrice>();

            var jsonReaderCollection = new AcoesJsonReaderPriceCollection()
            {
                Value = new List<AcoesJsonReaderPrice>()
            };

            foreach (var acao in acoes)
                jsonReaderCollection.Value.Add(acoesJsonReaderPriceBuilder.With(x => x.S, acao).Create());

            var gatewayService = new SerializationService();
            var jsonObject = JsonConvert.SerializeObject(jsonReaderCollection);

            // Act
            var result = gatewayService.DeserializeJson(jsonObject);

            // Assert
            result.Should().BeEquivalentTo(jsonReaderCollection);
        }
    }
}
