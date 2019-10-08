using System.Text;

namespace DzhWeatherDownloader
{
    public interface ISerializer
    {
        HydroMeteorologyData Serialize(WeatherPrediction data);
    }

    public class Serializer : ISerializer
    {
        public HydroMeteorologyData Serialize(WeatherPrediction data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var wea in data.Weather)
            {
                builder.Append(wea.Phenomenon);builder.Append(",");
                builder.Append(wea.Visibility);builder.Append(",");
                builder.Append(wea.WindDirection);builder.Append(",");
                builder.Append(wea.WindPower);builder.Append(",");
            }
            HydroMeteorologyData model = new HydroMeteorologyData
            {
                Location = data.Id,
                Content = builder.ToString()
            };
            return model;
        }
    }
}
