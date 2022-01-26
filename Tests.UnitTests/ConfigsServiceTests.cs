using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockMarket.Services;
using System;
using System.IO;
using System.Reflection;

namespace Tests.UnitTests
{
    [TestClass]
    public class ConfigsServiceTests
    {
        [TestMethod]
        public void Load_WhenConfigFileExists_ShouldLoadConfigs()
        {
            // Arrange
            try
            {
                this.EnsureConfigFileExists();

                var configsService = new ConfigsService();

                // Act
                configsService.Load();

                // Assert
                configsService.Acoes.Should().Be("IBOV;PETR4");
                configsService.Opening.Should().Be(new TimeSpan(10, 30, 0));
                configsService.Closing.Should().Be(new TimeSpan(18, 0, 0));
                configsService.CandlePeriod.Should().Be(60);
            }
            finally
            {
                this.EnsureConfigFileDoesNotExist();
            }
        }

        [TestMethod]
        public void Load_WhenConfigFileDoesNotExists_ShouldLoadDefaultConfigs()
        {
            // Arrange
            this.EnsureConfigFileDoesNotExist();

            var configsService = new ConfigsService();

            // Act
            configsService.Load();

            // Assert
            configsService.Acoes.Should().Be("IBOV");
            configsService.Opening.Should().Be(new TimeSpan(10, 0, 0));
            configsService.Closing.Should().Be(new TimeSpan(18, 30, 0));
            configsService.CandlePeriod.Should().Be(30);
        }

        [TestMethod]
        public void Save_WhenSaveConfigs_ShouldPersistTheNewConfigs()
        {
            // Arrange
            try
            {
                this.EnsureConfigFileDoesNotExist();

                var configsService = new ConfigsService();
                configsService.Acoes = "TEST";
                configsService.Opening = new TimeSpan(1, 0, 0);
                configsService.Closing = new TimeSpan(5, 5, 5);
                configsService.CandlePeriod = 8;

                // Act
                configsService.Save();
                configsService.Load();

                // Assert
                configsService.Acoes.Should().Be("TEST");
                configsService.Opening.Should().Be(new TimeSpan(1, 0, 0));
                configsService.Closing.Should().Be(new TimeSpan(5, 5, 5));
                configsService.CandlePeriod.Should().Be(8);
            }
            finally
            {
                this.EnsureConfigFileDoesNotExist();
            }
        }

        private void EnsureConfigFileExists()
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tests.UnitTests.TestFiles.config.ini"))
            using (var file = new FileStream("config.ini", FileMode.Create, FileAccess.Write))
                resource.CopyTo(file);
        }

        private void EnsureConfigFileDoesNotExist()
        {
            if (File.Exists("config.ini"))
                File.Delete("config.ini");
        }
    }
}
