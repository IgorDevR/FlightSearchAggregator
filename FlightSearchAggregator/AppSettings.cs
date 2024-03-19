namespace FlightSearchAggregator
{
    public class AppSettings
    {
        public string FlyQuestBaseUrl { get; set; }

        public string SkyTrailsBaseUrl { get; set; }
        public Guid ServiceId { get; set; }
        public string AppBaseUrlHttps { get; set; }
        public string AppBaseUrlHttp { get; set; }
        public string AppOuterUrl { get; set; }
    }
}