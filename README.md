# WebClient_DI
webClient下载气象数据，使用了dotnet 的DI（IServiceProvider）
## BuildDI()
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



IServiceProvider serviceProvider = BuildDI();
            logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Dzh");

            var manager = serviceProvider.GetRequiredService<IRunnable>();
            manager.Startup();
