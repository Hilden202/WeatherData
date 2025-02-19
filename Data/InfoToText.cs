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
            public static double CalculateAverageMoldRisk(List<WeatherData> data) =>
                data.Any() ? data.Average(w => w.RiskOfMold) : 0;

            public static double DeligateThis(CalculateWeatherMonthly calculate, List<Data.WeatherData> weatherList)
            {
                return calculate(weatherList);
            }
        }

        public static class WriteInfo
        {
            public static void WriteTextFile(List<Data.WeatherData> weatherList)
            {
                using (StreamWriter writer = new StreamWriter("../../../Files/MonthlyData.txt"))
                {
                    var monthlyData = weatherList
                        .GroupBy(w => $"{w.Date.Year}-{w.Date.Month:D2}")
                        .ToDictionary(g => g.Key, g => g.ToList());

                    CalculateWeatherMonthly monthlyTemp = WeatherCalculations.CalculateAverageTemp;
                    CalculateWeatherMonthly monthlyHumid = WeatherCalculations.CalculateAverageHumidity;
                    CalculateWeatherMonthly monthlyRiskOfMold = WeatherCalculations.CalculateAverageMoldRisk;

                    foreach (var month in monthlyData)
                    {
                        var insideData = month.Value.Where(w => w.Location == "Inne").ToList();
                        var outsideData = month.Value.Where(w => w.Location == "Ute").ToList();

                        double avgTempInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageTemp, insideData);
                        double avgTempOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageTemp, outsideData);

                        double avgHumuidityInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageHumidity, insideData);
                        double avgHumuidityOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageHumidity, outsideData);

                        double avgMoldRiskInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageMoldRisk, insideData);
                        double avgMoldRiskOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageMoldRisk, outsideData);


                        writer.WriteLine($"\t\tDate: {month.Key}");
                        writer.WriteLine($"Location: {insideData.First().Location}\t\t{outsideData.First().Location}");
                        writer.WriteLine($"Temperature: {avgTempInside:F2} °C\t{avgTempOutside:F2} °C");
                        writer.WriteLine($"Humidity: {avgHumuidityInside:F2}% \t{avgHumuidityOutside:F2}%");
                        writer.WriteLine($"Mold Risk: {avgMoldRiskInside:F2} \t{avgMoldRiskOutside:F2}");
                        writer.WriteLine(); // Blank line for better readability
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        string season = (i == 0) ? "Höst" : "Vinter";
                        string StartSeason = CalculateData.SeasonControll.SeasonStarted(weatherList, season);

                        writer.WriteLine(StartSeason);
                        writer.WriteLine();
                        
                    }
                    writer.WriteLine("Våran Formula för mögel: (humidity - 78) * (temp / 15) / 0.22");
                }
            }
        }
    }
}
