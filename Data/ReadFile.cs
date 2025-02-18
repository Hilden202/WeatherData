using System.Globalization;
using System.Text.RegularExpressions;
using static WeatherData.Data.WeatherData;

namespace WeatherData.Data
{
    public class ReadFile
    {
        public static List<WeatherData> ReadWeatherInfo(string filePath)
        {
            var weatherManager = new WeatherDataManager();


            using (StreamReader reader = new StreamReader(filePath))
            {
                string lines;

                string currentDayInside = null;
                string currentDayOutside = null;

                double sumTemperatureInside = 0; // avarage day Inside
                double sumHumidityInside = 0;

                //---------------------------------------------------------------
                double sumTemperatureOutside = 0; // avarage day Outside
                double sumHumidityOutside = 0;

                int countInside = 0;
                int countOutside = 0;

                foreach (var line in File.ReadAllLines(filePath))
                { 
                    if (!Regex.IsMatch(line, @"^(2016-05-\d{2}|2017-01-\d{2})"))
                    {
                        Regex regex = new Regex(@"(\d{4})-(\d{2})-(\d{2}) \d{2}:\d{2}:\d{2},(Ute|Inne),(-?\d{1,2}\.\d+),(\d{1,3})");

                        //Console.WriteLine(lines);
                        
                        Match match = regex.Match(line);  
                        string monthj = match.Groups[2].Value;

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

                            if (currentDayOutside == null)
                            {
                                currentDayOutside = day;
                                currentDayInside = day;
                            }

                            if (day != currentDayInside && location == "Inne")
                            {
                                double aveTemp = sumTemperatureInside / countInside;
                                double aveHumidity = sumHumidityInside / countInside;

                                string riskStatus = CalculateMoldRisk(aveHumidity, aveTemp).Item1;
                                int riskOfMold = (int)CalculateMoldRisk(aveHumidity, aveTemp).Item2;


                                weatherManager.AddWeatherData(aveTemp, aveHumidity, riskOfMold, riskStatus, date, location);

                                currentDayInside = day;
                                sumTemperatureInside = 0;
                                sumHumidityInside = 0;
                                countInside = 0;

                            }
                            if (day != currentDayOutside && location == "Ute")
                            {
                                double aveTemp = sumTemperatureOutside / countOutside;
                                double aveHumidity = sumHumidityOutside / countOutside;


                                string riskStatus = CalculateMoldRisk(aveHumidity, aveTemp).Item1;
                                int riskOfMold = (int)CalculateMoldRisk(aveHumidity, aveTemp).Item2;


                                weatherManager.AddWeatherData(aveTemp, aveHumidity, riskOfMold, riskStatus, date, location);
                                currentDayOutside = day;
                                sumTemperatureOutside = 0;
                                sumHumidityOutside = 0;
                                countOutside = 0;
                            }

                            if (location == "Ute")
                            {
                                sumTemperatureOutside += temperature;
                                sumHumidityOutside += humidity;
                                countOutside++;
                            }
                            else if (location == "Inne")
                            {
                                sumTemperatureInside += temperature;
                                sumHumidityInside += humidity;
                                countInside++;
                            }
                        }
                    }
                }
            }


            return weatherManager.WeatherList;
        }

        public static (string, double) CalculateMoldRisk(double humidity, double temp)
        {
            double moldRisk = (humidity - 78) * (temp / 15) / 0.22;

            if (moldRisk < 0)
            {
                moldRisk = 0;
            }
            if(humidity < 78 && temp < 15)
            {
                return ("Temperature and Humidity is below requirement for mold to grow.", moldRisk);
            }
            else if (humidity < 78)
            {
                return ("Humidity is below requirement for mold to grow.", moldRisk);
            }
            else if (temp < 15)
            {
                return ("Temperature is below requirement for mold to grow.", moldRisk);
            }

            string riskLevel = moldRisk > 50 ? "High Risk of mold" + moldRisk : "Low Risk of mold" + moldRisk;

            return (riskLevel, moldRisk);
        }



    }
}