using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DzhWeatherDownloader
{
    public interface IDownloader
    {
        WeatherPrediction[] Download();
    }

    public class WeatherDownloader : IDownloader
    {
        private readonly ILogger _logger;
        private readonly HashSet<string> _locations = null;
        private readonly WebapiClient.WebApiClient<WeatherPrediction, string> _client = null;

        public WeatherDownloader(ILogger logger,IConfiguration configuration)
        {
            this._logger = logger;
            var locals = configuration["Locations"];
            _logger.LogInformation($"get local:{locals}");
            _locations = new HashSet<string>(locals.Split(",", System.StringSplitOptions.RemoveEmptyEntries));

            _client = new WebapiClient.WebApiClient<WeatherPrediction, string>(configuration["DZHWeatherService"], "weather", p => p.Id, (p, id) => p.Id = id);
        }

        public WeatherPrediction[] Download()
        {
            WeatherPrediction[] weatherPredictions = _client.QueryDataList(string.Empty, null);
            return weatherPredictions.Where(p=>_locations.Contains(p.Id)).ToArray();
        }
    }
}
