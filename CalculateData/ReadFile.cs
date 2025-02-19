using System.Globalization;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using static WeatherData.Data.WeatherData;
using static WeatherData.Data.InfoToText;

namespace WeatherData.CalculateData
{
    public class ReadFile
    {
        public static List<Data.WeatherData> ReadWeatherInfo(string filePath)
        {
            var weatherManager = new WeatherDataManager();
            var temporaryList = new List<Data.WeatherData>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (!Regex.IsMatch(line, @"^(2016-05-\d{2}|2017-01-\d{2})"))
                    {
                        Regex regex = new Regex(@"(\d{4})-(\d{2})-(\d{2}) \d{2}:\d{2}:\d{2},(Ute|Inne),(-?\d{1,2}\.\d+),(\d{1,3})");
                        Match match = regex.Match(line);

                        if (match.Success)
                        {
                            string year = match.Groups[1].Value; // Om vi ska rakna flera ar
                            string month = match.Groups[2].Value;
                            string day = match.Groups[3].Value;
                            string location = match.Groups[4].Value;
                            double temperature = double.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);
                            double humidity = double.Parse(match.Groups[6].Value);

                            if (temperature > 30 || temperature < -20)
                            {
                                continue;
                            }

                            DateOnly date;
                            string dateString = year + "-" + month + "-" + day;
                            try
                            {
                                date = WeatherDataManager.ConvertToDateOnly(dateString);
                            }
                            catch (ArgumentException ex)
                            {
                                continue;
                            }

                            double riskOfMold = CalculateMoldRisk(humidity, temperature);

                            string riskStatus = string.Empty;
                            temporaryList.Add(new Data.WeatherData(temperature,humidity, riskOfMold,riskStatus,date,location));
                        }
                    }
                }
                var groupedDay = temporaryList.GroupBy(p => $"{p.Date.Year}-{p.Date.Month}-{p.Date.Day}").ToList();

                foreach (var specificDay in groupedDay)
                {
                    var insideData = specificDay.Where(p => p.Location == "Inne").ToList();
                    var outsideData = specificDay.Where(p => p.Location == "Ute").ToList();

                    double avgTempInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageTemp, insideData);
                    double avgTempOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageTemp, outsideData);

                    double avgHumuidityInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageHumidity, insideData);
                    double avgHumuidityOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageHumidity, outsideData);

                    double avgMoldRiskInside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageMoldRisk, insideData);
                    double avgMoldRiskOutside = WeatherCalculations.DeligateThis(WeatherCalculations.CalculateAverageMoldRisk, outsideData);

                    string riskStatusInside = WeatherCalculations.CalculateMoldStatus(avgTempInside, avgTempInside, avgHumuidityInside);
                    string riskStatusOutside = WeatherCalculations.CalculateMoldStatus(avgTempOutside, avgTempOutside, avgHumuidityOutside);


                    weatherManager.AddWeatherData(avgTempInside, avgHumuidityInside, avgMoldRiskInside, riskStatusInside, insideData.First().Date, insideData.First().Location);
                    weatherManager.AddWeatherData(avgTempOutside, avgHumuidityOutside, avgMoldRiskOutside, riskStatusOutside, outsideData.First().Date, outsideData.First().Location);

                }
            }
            return weatherManager.WeatherList;
        }
        
        public static  double CalculateMoldRisk(double humidity, double temp)
        {
            double moldRisk = (humidity - 78) * (temp / 15) / 0.22;

            return moldRisk;
        }



    }
}