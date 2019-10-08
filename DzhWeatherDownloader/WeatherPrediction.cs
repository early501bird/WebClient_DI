namespace DzhWeatherDownloader
{
    public class WeatherPrediction
    {
        public string Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class Weather
    {
        /// <summary>
        /// 气象
        /// </summary>
        public string Phenomenon { get; set; }
        /// <summary>
        /// 能见度
        /// </summary>
        public string Visibility { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        public string WindDirection { get; set; }
        /// <summary>
        /// 风力
        /// </summary>
        public string WindPower { get; set; }
        /// <summary>
        /// 浪高
        /// </summary>
        public string WavHigh { get; set; }
    }
}