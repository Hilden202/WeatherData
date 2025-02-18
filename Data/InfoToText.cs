namespace WeatherData.Data
{
    internal class InfoToText
    {

        public delegate double CalculateWeatherMonthly(List<Data.WeatherData> weatherList);

        public static class WeatherCalculations
        {
            public static double CalculateAverageTemp(List<WeatherData> data) =>
                data.Any() ? data.Average(w => w.AveTemp) : 0;

            public static double CalculateAverageHumidity(List<WeatherData> data) =>
                data.Any() ? data.Average(w => w.AveHumidity) : 0;
        }


        class WriteInfo
        {
            public static void WriteTextFile(List<Data.WeatherData> weatherList)
            {

                var monthlyData = weatherList
                .GroupBy(w => $"{w.Date.Year}-{w.Date.Month:D2}")
                .ToDictionary(g => g.Key, g => g.ToList());

                CalculateWeatherMonthly monthlyTemp = WeatherCalculations.CalculateAverageTemp;
                CalculateWeatherMonthly monthlyHumid = WeatherCalculations.CalculateAverageHumidity;

            }
        }


    }
}
