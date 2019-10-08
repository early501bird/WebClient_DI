using DzhWeatherDownloader.WebapiClient;
using Microsoft.Extensions.Configuration;

namespace DzhWeatherDownloader
{
    public interface IStorager
    {
        void Storage(HydroMeteorologyData data);
    }

    public class Storager : IStorager
    {
        private readonly HydroMeteorologyClient _client;

        public Storager(IConfiguration configuration)
        {
            var serverUrl = configuration["VDESWebService"];
            _client = new HydroMeteorologyClient(serverUrl);
        }

        public void Storage(HydroMeteorologyData data)
        {
            _client.Add(data);
        }

        class HydroMeteorologyClient:WebApiClient<HydroMeteorologyData,string>
        {
            public HydroMeteorologyClient(string server):base(server, "BroadcastContentHydroMeteorology",p=>p.Location,(p,location)=>p.Location=location)
            {

            }

            public void Add(HydroMeteorologyData data)
            {
                process(_client.PostAsJsonAsync($"{ControllerUrl}/Add", data));
            }
        }
    }

}
