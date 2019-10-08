using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DzhWeatherDownloader
{
    interface IRunnable
    {
        void Startup();
        void Shutdown();
    }

    public class Manager : IRunnable
    {
        public readonly ILogger<Manager> _logger = null;
        private readonly IConfiguration _configuration;
        private readonly IDownloader _downloader;
        private readonly ISerializer _serializer;
        private readonly IStorager _storager;
        private CancellationTokenSource cancellationTokenSource;
        public Manager(ILogger<Manager> logger,IConfiguration configuration,IDownloader downloader,ISerializer serializer,IStorager storager)
        {
            _logger = logger;
            _configuration = configuration;
            this._downloader = downloader;
            this._serializer = serializer;
            this._storager = storager;
        }

        public void Shutdown()
        {
            cancellationTokenSource.Cancel();
            _logger.LogInformation("manager shutdown ...");
        }

        public void Startup()
        {
            _logger.LogInformation("manager startup ...");
            cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(new Action(Work),cancellationTokenSource.Token);
        }

        private void Work()
        {
            double seconds = 0;
            if (!double.TryParse(_configuration["Interval"], out seconds)) seconds = 3600;
            while (true)
            {
                var weatherPres = _downloader.Download();
                _logger.LogInformation($"get weather:{JsonConvert.SerializeObject(weatherPres)}");
                foreach (var weatherPre in weatherPres)
                {
                    var model = _serializer.Serialize(weatherPre);
                    _storager.Storage(model);
                    _logger.LogInformation($"storage:{ model.Location},{model.Content}");
                }

                Thread.Sleep(TimeSpan.FromSeconds(seconds));
            }

        }
    }
}
