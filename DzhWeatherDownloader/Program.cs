using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DzhWeatherDownloader
{
    class Program
    {
        static ILogger logger = null;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            IServiceProvider serviceProvider = BuildDI();
            logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Dzh");

            var manager = serviceProvider.GetRequiredService<IRunnable>();
            manager.Startup();
            
            Console.ReadLine();
            manager.Shutdown();
        }


        private static ServiceProvider BuildDI()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsetting.json").Build());
            services.AddLogging(p => p.SetMinimumLevel(LogLevel.Trace));
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<IRunnable, Manager>();
            ServiceProvider provider = services.BuildServiceProvider();

            var logFactory = provider.GetRequiredService<ILoggerFactory>();
            logFactory.AddNLog(new NLogProviderOptions() { CaptureMessageProperties = true, CaptureMessageTemplates = true });
            NLog.LogManager.LoadConfiguration("NLog.config");

            return provider;
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.LogError(e.ExceptionObject.ToString());
            Process.GetCurrentProcess().Kill();
        }
    }
}
