using Microsoft.Extensions.DependencyInjection;
using StockMarket.Services;
using System;
using System.Net;
using System.Windows.Forms;

namespace StockMarket
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            ConfigureServices();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var configs = ServiceProvider.GetService<IConfigsService>();
            configs.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static IServiceProvider ServiceProvider { get; set; }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfigsService, ConfigsService>();
            services.AddTransient<IGatewayService, GatewayService>();
            services.AddTransient<ISerializationService, SerializationService>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
