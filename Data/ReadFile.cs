using System;
using System.Text.RegularExpressions;

namespace WeatherData.Data
{
    public class ReadFile
    {
        public static string path = "../../../Files/";
        public static void ReadAllWithIgnore(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(path + "ignore2016_05-2017_01.txt"))
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

        public static void ReadAllFromIgnoreToIndoor(string fileName)
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

        public static void SearchIndoorTemperatureByDate(string fileName)
        {
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                Console.Write("Sök på ett datum för att kolla inomhus medeltemperatur: ");
                string datum = Console.ReadLine();

                if (Regex.IsMatch(datum, @"^(2016-(0[6-9]|1[0-2])|2016-05-(3[1])|2017-01-(0[1-9]|10))[-/]\d{2}[-/]\d{2}$"))
                {
                    Console.WriteLine("Ogiltigt datumformat. Försök igen med formatet 'YYYY-MM-DD' eller 'YYYY/MM/DD'.");
                    return;
                }

                string line;
                double totalTemperature = 0;
                int temperatureCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(datum))  // Kontrollera om raden innehåller det angivna datumet
                    {
                        var match = Regex.Match(line, @"Inomhus temperatur:\s*(-?\d+(\.\d+)?)\s*°C");
                        if (match.Success)
                        {
                            double temperature = double.Parse(match.Groups[1].Value);
                            totalTemperature += temperature;
                            temperatureCount++;
                        }
                    }
                }

                if (temperatureCount > 0)
                {
                    double averageTemperature = totalTemperature / temperatureCount;
                    Console.WriteLine("Medeltemperaturen för " + datum + " är " + averageTemperature + "°C");
                }
                else
                {
                    Console.WriteLine("Inga temperaturer funna för " + datum + ".");
                }
            }
        }
    }
}


