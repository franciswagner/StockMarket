using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockMarket;
using StockMarket.Services;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Tests.UnitTests
{
    [TestClass]
    public class PersistenceServiceTests
    {
        [TestMethod]
        public void LoadFromFiles_WhenDataFilesExist_ShouldLoadThem()
        {
            try
            {
                // Arrange
                this.EnsureDataFilesExist();

                var ticketNames = new string[] { "IBOV", "PETR4", "ABC" };

                var configsService = new Mock<IConfigsService>();
                configsService.Setup(x => x.MaxCandlesInGraph).Returns(10);
                configsService.Setup(x => x.CandlePeriod).Returns(10);
                var gatewayService = new PersistenceService(configsService.Object);

                // Act
                var result = gatewayService.LoadFromFiles(ticketNames);

                // Assert
                result.Count.Should().Be(2);
                result.Single(x => x.Name == "IBOV").Acoes.Count.Should().Be(17);
                result.Single(x => x.Name == "PETR4").Acoes.Count.Should().Be(14);
                result.Single(x => x.Name == "IBOV").Acoes.First().Price.Should().Be(107914.62M);
                result.Single(x => x.Name == "PETR4").Acoes.First().Price.Should().Be(26.39M);
            }
            finally
            {
                this.EnsureDataFilesDoNotExist();
            }
        }

        [TestMethod]
        public void SaveToFile_WhenHaveDataToPersist_ShouldWriteInFile()
        {
            try
            {
                // Arrange
                this.EnsureDataFilesDoNotExist();

                var acao = new Fixture().Build<Acao>().Create();

                var configsService = new Mock<IConfigsService>();
                var gatewayService = new PersistenceService(configsService.Object);

                // Act
                gatewayService.SaveToFile("ABC", acao);

                // Assert
                var content = File.ReadAllText(@"DataFiles\ABC.txt").Trim();
                content.Count(x => x == ';').Should().Be(9);
                content.Split(';')[0].Should().Be("ABC");
                content.Split(';')[1].Should().Be(acao.RequestedDate.ToString());
                content.Split(';')[2].Should().Be(acao.Date.ToString());
                content.Split(';')[3].Should().Be(acao.OppeningPrice.ToString());
                content.Split(';')[4].Should().Be(acao.Price.ToString());
                content.Split(';')[5].Should().Be(acao.MinimunPrice.ToString());
                content.Split(';')[6].Should().Be(acao.MaximunPrice.ToString());
                content.Split(';')[7].Should().Be(acao.AveragePrice.ToString());
                content.Split(';')[8].Should().Be(acao.Volume.ToString());
                content.Split(';')[9].Should().Be(acao.ClosedPrice.ToString());
            }
            finally
            {
                this.EnsureDataFilesDoNotExist();
            }
        }

        private void EnsureDataFilesExist()
        {
            if (!Directory.Exists("DataFiles"))
                Directory.CreateDirectory("DataFiles");

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tests.UnitTests.DataFiles.IBOV.txt"))
            using (var file = new FileStream(@"DataFiles\IBOV.txt", FileMode.Create, FileAccess.Write))
                resource.CopyTo(file);

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tests.UnitTests.DataFiles.PETR4.txt"))
            using (var file = new FileStream(@"DataFiles\PETR4.txt", FileMode.Create, FileAccess.Write))
                resource.CopyTo(file);
        }

        private void EnsureDataFilesDoNotExist()
        {
            if (Directory.Exists("DataFiles"))
                Directory.Delete("DataFiles", true);
        }
    }
}
