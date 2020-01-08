using System;
using System.Collections.Generic;

namespace PriceMonitor
{
    public class AcoesJsonReaderPriceCollection
    {
        public List<AcoesJsonReaderPrice> Value { get; set; }
    }

    public class AcoesJsonReaderPrice
    {
        /// <summary>
        /// Nome da acão
        /// </summary>
        public string S { get; set; }

        /// <summary>
        /// Data e hora
        /// </summary>
        public DateTime UT { get; set; }

        /// <summary>
        /// Objeto com os preços
        /// </summary>
        public AcoesJsonReader Ps { get; set; }
    }

    public class AcoesJsonReader
    {
        /// <summary>
        /// Preco na abertura do mercado
        /// </summary>
        public decimal OP { get; set; }
        
        /// <summary>
        /// Preco no fechamento do mercado
        /// </summary>
        public decimal CP { get; set; }

        /// <summary>
        /// Preco atual
        /// </summary>
        public decimal P { get; set; }

        /// <summary>
        /// Preco médio
        /// </summary>
        public decimal AvP { get; set; }

        /// <summary>
        /// Preco mínimo
        /// </summary>
        public decimal MnP { get; set; }

        /// <summary>
        /// Preco máximo
        /// </summary>
        public decimal MxP { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public decimal V { get; set; }
    }
}