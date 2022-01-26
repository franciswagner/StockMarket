using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceMonitor;
using StockMarket;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests.UnitTests
{
    [TestClass]
    public class AbstractRowTests
    {
        [TestMethod]
        public void UpdateValue_WhenCallAUpdateInEmptyAbstractRow_ShouldLoadAllValues()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));

            var acao = fixture
                .Build<Acao>()
                .With(x => x.RequestedDate, new DateTime(2022, 1, 1, 11, 0, 0))
                .Create();

            var abstractRow = new AbstractRow();

            // Act
            abstractRow.UpdateValue(acao);

            // Assert
            abstractRow.Name.Should().BeNullOrEmpty();
            abstractRow.Closing.Should().Be(acao.ClosedPrice_Formated);
            abstractRow.Opening.Should().Be(acao.OppeningPrice_Formated);
            abstractRow.Minimun.Should().Be(acao.MinimunPrice_Formated);
            abstractRow.Maximun.Should().Be(acao.MaximunPrice_Formated);
            abstractRow.Time1030.Should().BeNullOrEmpty();
            abstractRow.Time1100.Should().NotBeNullOrEmpty();
            abstractRow.Time1130.Should().BeNullOrEmpty();
            abstractRow.Time1200.Should().BeNullOrEmpty();
            abstractRow.Time1230.Should().BeNullOrEmpty();
            abstractRow.Time1300.Should().BeNullOrEmpty();
            abstractRow.Time1330.Should().BeNullOrEmpty();
            abstractRow.Time1400.Should().BeNullOrEmpty();
            abstractRow.Time1430.Should().BeNullOrEmpty();
            abstractRow.Time1500.Should().BeNullOrEmpty();
            abstractRow.Time1530.Should().BeNullOrEmpty();
            abstractRow.Time1600.Should().BeNullOrEmpty();
            abstractRow.Time1630.Should().BeNullOrEmpty();
            abstractRow.Time1700.Should().BeNullOrEmpty();
            abstractRow.Time1730.Should().BeNullOrEmpty();
            abstractRow.Time1800.Should().BeNullOrEmpty();
            abstractRow.RentabilidadePerc.Should().NotBeNullOrEmpty();
            abstractRow.RentabilidadeValor.Should().NotBeNullOrEmpty();
            abstractRow.Volume.Should().Be(acao.Volume_Formated);
        }

        [TestMethod]
        public void UpdateValue_When_Should()
        {
            // Arrange
            var refDate = new DateTime(2022, 1, 1, 11, 1, 0);
            var fixture = new Fixture();
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(1, 100));

            var acoesBuilder = fixture
                .Build<Acao>();

            var acao = acoesBuilder
                .With(x => x.RequestedDate, refDate)
                .With(x => x.Date, refDate)
                .Create();

            var acoesCollection = new AcoesCollection()
            {
                Name = "ABC",
                Acoes = new List<Acao>() { acoesBuilder.With(x => x.RequestedDate, refDate.AddMinutes(1)).Create() }
            };

            var abstractRow = new AbstractRow(refDate, acoesCollection);

            // Act
            abstractRow.UpdateValue(acao);

            // Assert
            abstractRow.Name.Should().Be(acoesCollection.Name);
            abstractRow.Closing.Should().Be(acao.ClosedPrice_Formated);
            abstractRow.Opening.Should().Be(acao.OppeningPrice_Formated);
            abstractRow.Minimun.Should().Be(acao.MinimunPrice_Formated);
            abstractRow.Maximun.Should().Be(acao.MaximunPrice_Formated);
            abstractRow.Time1030.Should().BeNullOrEmpty();
            abstractRow.Time1100.Should().BeNullOrEmpty();
            abstractRow.Time1130.Should().Be(acao.Price_Formated);
            abstractRow.Time1200.Should().BeNullOrEmpty();
            abstractRow.Time1230.Should().BeNullOrEmpty();
            abstractRow.Time1300.Should().BeNullOrEmpty();
            abstractRow.Time1330.Should().BeNullOrEmpty();
            abstractRow.Time1400.Should().BeNullOrEmpty();
            abstractRow.Time1430.Should().BeNullOrEmpty();
            abstractRow.Time1500.Should().BeNullOrEmpty();
            abstractRow.Time1530.Should().BeNullOrEmpty();
            abstractRow.Time1600.Should().BeNullOrEmpty();
            abstractRow.Time1630.Should().BeNullOrEmpty();
            abstractRow.Time1700.Should().BeNullOrEmpty();
            abstractRow.Time1730.Should().BeNullOrEmpty();
            abstractRow.Time1800.Should().BeNullOrEmpty();
            abstractRow.RentabilidadePerc.Should().NotBeNullOrEmpty();
            abstractRow.RentabilidadeValor.Should().NotBeNullOrEmpty();
            abstractRow.Volume.Should().Be(acao.Volume_Formated);
        }
    }
}
