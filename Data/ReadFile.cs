using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WeatherData.Data
{
    public class ReadFile
    {
        public static string path = "../../../Files/";
        public static void ReadAllAndCreateFileTempdata(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(path + "Tempdata.txt"))
            {
                using (StreamReader reader = new StreamReader(path + fileName))
                {
                    string line = reader.ReadLine();
                    int rowCount = 0;
                    while (line != null)
                    {
                        if (!Regex.IsMatch(line, @"^(2016-(0[6-9]|1[0-2])|2016-05-(3[1])|2017-01-(0[1-9]|10))\s"))
                        {
                            //Console.WriteLine(rowCount + " " + line);
                            writer.WriteLine(rowCount + " " + line);

                        }
                        rowCount++;
                        line = reader.ReadLine();
                    }
                }
            }
        }
        public static void SortDataBy(string fileName, string pattern, bool IsTemp)
        {
            //string temperaturePattern = @"(\d{4}-\d{2}-\d{2}).*((?<=,)\d{1,2}.\d)";
            //string humidityPattern = @"(\d{4}-\d{2}-\d{2}).*((?<=,)\d{1,2})";
            //List<string> list = new List<string>(); 
            List<Tuple<double, string>> list = new List<Tuple<double, string>>();
            using (StreamReader reader = new StreamReader(path + fileName))
            {

                string line = reader.ReadLine();
                while (line != null)
                {
                    foreach (Match m in Regex.Matches(line, pattern))
                    {
                        if (m.Success)
                        {
                            double temperature = double.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
                            string date = m.Groups[1].Value.ToString();
                            // Group[2] is the temperature, Group[1] is the date
                            //list.Add(m.Groups[2].Value.ToString() + "\t\t" + m.Groups[1].Value.ToString());
                            list.Add(new Tuple<double, string>(temperature, date));
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            //list.Sort(); // Sort list 
            list.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            // If sorted by temperature
            if (IsTemp)
            {
                Console.WriteLine("Temperatur\tDatum");
                list.Reverse(); // Reverse list to start with highest value
            }
            // If sorted by humidity
            else
            {
                Console.WriteLine("Luftfuktighet\tDatum");
            }
            // Write sorted data
            //foreach (var line in list)
            //{
            //    Console.WriteLine(line);
            //}
            foreach (var entry in list)
            {
                //Console.WriteLine(line);
                Console.WriteLine(entry.Item1.ToString("0.0", CultureInfo.InvariantCulture) + "\t\t" + entry.Item2);
            }
        }

        public static void ReadAllTest(string filePath, List<WeatherData> weatherList)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string lines = reader.ReadLine();

                string currentDay = null;
                string currentMonth = null;

                double sumTemperatureInside = 0; // avarage day Inside
                double sumHumidityInside = 0;

                //---------------------------------------------------------------
                double sumTemperatureOutside = 0; // avarage day Outside
                double sumHumidityOutside = 0;

                int countInside = 0;
                int countOutside = 0;

                while ((lines = reader.ReadLine()) != null)
                {
                    if (!Regex.IsMatch(lines, @"^(2016-(0[6-9]|1[0-2])|2016-05-(3[1])|2017-01-(0[1-9]|10))\s"))
                    {
                        Regex regex = new Regex(@"(\d{4})-(\d{2})-(\d{2}) \d{2}:\d{2}:\d{2},(Ute|Inne),(\d{2}.\d|\d{1}.\d),(\d{2})");

                        string line = reader.ReadLine();
                        Match match = regex.Match(line);

                        if (match.Success)
                        {
                            string year = match.Groups[1].Value; // Om vi ska rakna flera ar
                            string month = match.Groups[2].Value;
                            string day = match.Groups[3].Value;
                            string location = match.Groups[4].Value;
                            double temperature = double.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);
                            double humidity = double.Parse(match.Groups[6].Value);


                            DateOnly date;
                            string dateString = year + "-" + month + "-" + day;
                            try
                            {
                                date = WeatherData.ConvertToDateOnly(dateString);
                            }
                            catch (ArgumentException ex)
                            {


                                break;
                            }



                            if (currentDay == null)
                            {
                                currentDay = day;
                                currentMonth = month;
                            }

                            if (day != currentDay)
                            {
                                if (location == "Inne")
                                {
                                    double aveTemp = sumTemperatureInside / countInside;
                                    double aveHumidity = sumHumidityInside / countInside;

                                    string riskOfMold = CalculateMoldRisk(aveHumidity, aveTemp);

                                    var weatherData = new WeatherData(
                                        temperature = aveTemp,
                                        humidity = aveHumidity,
                                        riskOfMold = riskOfMold,
                                        date = date,
                                        location = location
                                        );
                                    weatherList.Add(weatherData);

                                    currentDay = day;
                                    sumTemperatureInside = 0;
                                    sumHumidityInside = 0;
                                    countInside = 0;

                                }
                                else if (location == "Ute")
                                {
                                    double aveTemp = sumTemperatureOutside / countOutside;
                                    double aveHumidity = sumHumidityOutside / countOutside;


                                    string riskOfMold = CalculateMoldRisk(aveHumidity, aveTemp);

                                    var weatherData = new WeatherData(
                                     temperature = aveTemp,
                                     humidity = aveHumidity,
                                     riskOfMold = riskOfMold,
                                     date = date,
                                     location = location
                                     );
                                    weatherList.Add(weatherData);

                                    currentDay = day;
                                    sumTemperatureOutside = 0;
                                    sumHumidityOutside = 0;
                                    countOutside = 0;
                                }

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


                lines = reader.ReadLine();
            }
        }

        public static void CreateFileIndoor(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(path + "InneTemperaturer.txt"))
            {
                using (StreamReader reader = new StreamReader(path + fileName))
                {
                    string line = reader.ReadLine();
                    int rowCount = 0;
                    while (line != null)
                    {
                        if (Regex.IsMatch(line, @"\bInne\b"))
                        {
                            //Console.WriteLine(rowCount + " " + line);
                            writer.WriteLine(rowCount + " " + line);
                        }
                        rowCount++;
                        line = reader.ReadLine();
                    }
                }
            }
        }

        public static void CreateFileOutdoor(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(path + "UteTemperaturer.txt"))
            {
                using (StreamReader reader = new StreamReader(path + fileName))
                {
                    string line = reader.ReadLine();
                    double totalTemperature = 0;
                    int rowCount = 0;
                    while (line != null)
                    {
                        if (Regex.IsMatch(line, @"\bUte\b"))
                        {
                            // Console.WriteLine(rowCount + " " + line);
                            writer.WriteLine(rowCount + " " + line);
                        }
                        rowCount++;
                        line = reader.ReadLine();
                    }
                }
            }
        }
        public static void CreateFileAverageTempOutside(string fileName)
        {
            string inputFile = Path.Combine(path, fileName);
            string outputFile = Path.Combine(path, "AverageTemperaturerUte.txt");

            Dictionary<string, List<double>> dailyTemperatures = new Dictionary<string, List<double>>();

            using (StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = Regex.Match(line, @"(\d{4}-\d{2}-\d{2}).*?(-?\d+\.\d+)");

                    if (match.Success)
                    {
                        string date = match.Groups[1].Value;
                        double temperature = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                        if (!dailyTemperatures.ContainsKey(date))
                        {
                            dailyTemperatures[date] = new List<double>();
                        }
                        dailyTemperatures[date].Add(temperature);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                foreach (var entry in dailyTemperatures)
                {
                    string date = entry.Key;
                    double averageTemp = entry.Value.Average();

                    string result = date + ": " + averageTemp.ToString("0.0");

                    //Console.WriteLine(result);
                    writer.WriteLine(result);
                }
            }
        }
        public static void SearchOutdoorTempAndHumidityByDate(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                Console.Write("Sök på ett datum för att kolla medeltemperatur och luftfuktighet den dagen: ");
                string datum = Console.ReadLine();

                if (!Regex.IsMatch(datum, @"\d{4}[-/]\d{2}[-/]\d{2}"))
                {
                    Console.WriteLine("Ogiltigt datumformat. Försök igen med formatet 'YYYY-MM-DD' eller 'YYYY/MM/DD'.");
                    return;
                }

                string line;
                double totalTemperature = 0;
                double totalHumidity = 0;
                int recordCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(datum))
                    {
                        var tempMatch = Regex.Match(line, @"(\d+\.\d+)"); // Matcher temperatur, t.ex. 19.4

                        var humidityMatch = Regex.Match(line, @"(\d+)$"); // Matcher sista siffran som luftfuktighet, t.ex. 35

                        if (tempMatch.Success && humidityMatch.Success)
                        {
                            double temperature = double.Parse(tempMatch.Groups[1].Value);
                            double humidity = double.Parse(humidityMatch.Groups[1].Value);

                            totalTemperature += temperature;
                            totalHumidity += humidity;
                            recordCount++;
                        }
                    }
                }

                if (recordCount > 0)
                {
                    double averageTemperature = totalTemperature / recordCount;
                    double averageHumidity = totalHumidity / recordCount;

                    Console.WriteLine("Medeltemperaturen för " + datum + " är " + averageTemperature.ToString("0.0") + "°C och luftfuktigheten är " + averageHumidity.ToString("0") + "%.");
                }
                else
                {
                    Console.WriteLine("Inga temperaturer eller luftfuktighet funna för " + datum + ".");
                }
            }
        }
        public static void SearchAvarageIndoorTempByDate(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                Console.Write("Sök på ett datum för att kolla medeltemperatur innomhus den dagen: ");
                string datum = Console.ReadLine();

                if (!Regex.IsMatch(datum, @"\d{4}[-/]\d{2}[-/]\d{2}"))
                {
                    Console.WriteLine("Ogiltigt datumformat. Försök igen med formatet 'YYYY-MM-DD' eller 'YYYY/MM/DD'.");
                    return;
                }

                string line;
                double totalTemperature = 0;
                int recordCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(datum))
                    {
                        var tempMatch = Regex.Match(line, @"(\d+\.\d+)");


                        if (tempMatch.Success)
                        {
                            double temperature = double.Parse(tempMatch.Groups[1].Value);

                            totalTemperature += temperature;
                            recordCount++;
                        }
                    }
                }

                if (recordCount > 0)
                {
                    double averageTemperature = totalTemperature / recordCount;

                    Console.WriteLine("Medeltemperaturen för " + datum + " är " + averageTemperature.ToString("0.0"));
                }
                else
                {
                    Console.WriteLine("Inga temperaturer funna för " + datum + ".");
                }
            }
        }


    }
}