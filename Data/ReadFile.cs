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

        public static void ReadAllTest(string fileName)
{
    using (StreamReader reader = new StreamReader(path + fileName))
    {
        string lines = reader.ReadLine();
        int rowCount = 0;
        string currentDay = null;
        string currentMonth = null;

        double sumTemperatureInside = 0; // avarage day Inside
        double sumHumidityInside = 0;
        
        double sumDayTempInside = 0;   //avarage month Inside
        double sumDayHumidityInside = 0;

        //---------------------------------------------------------------
        double sumTemperatureOutside = 0; // avarage day Outside
        double sumHumidityOutside = 0;

        double sumDayTempOutside = 0;   //avarage month Outside
        double sumDayHumidityOutside = 0;


        int countInside = 0;
        int countOutside = 0;
        int dayCountInside = 0;
        int dayCountOutside = 0;



        while (!reader.EndOfStream)
        {
            if (!Regex.IsMatch(lines, @"^(2016-(0[6-9]|1[0-2])|2016-05-(3[1])|2017-01-(0[1-9]|10))\s"))
            {
                Regex regex = new Regex(@"(\d{4})-(\d{2})-(\d{2}) \d{2}:\d{2}:\d{2},(Ute|Inne),(\d{2}.\d|\d{1}.\d),(\d{2})");

                string line = reader.ReadLine();
                Match match = regex.Match(line);

                if (match.Success)
                {
                   // string year = match.Groups[1].Value; // Om vi ska rakna flera ar
                    string month = match.Groups[2].Value;
                    string day = match.Groups[3].Value;
                    string location = match.Groups[4].Value;
                    int temperature = int.Parse(match.Groups[5].Value);
                    int humidity = int.Parse(match.Groups[6].Value);

                    if (currentDay == null)
                    {
                        currentDay = day;
                        currentMonth = month;
                    }

                    if (day != currentDay)
                    {
                        if(location == "Inne")
                        {
                            double aveDayTempInside = sumTemperatureInside / countInside;
                            double aveDayHumidityInside = sumHumidityInside / countInside;

                            sumDayHumidityInside += aveDayHumidityInside;
                            sumDayTempInside += aveDayTempInside;

                            currentDay = day;
                            sumTemperatureInside = 0;
                            sumHumidityInside = 0;
                            countInside = 0;
                            dayCountInside++;
                        }
                        else if (location == "Ute")
                        {
                            double aveDayTempOutside = sumTemperatureOutside / countOutside;
                            double aveDayHumidityOutside = sumHumidityOutside / countOutside;

                            sumDayHumidityOutside += aveDayHumidityOutside;
                            sumDayTempOutside += aveDayTempOutside;

                            currentDay = day;
                            sumTemperatureOutside = 0;
                            sumHumidityOutside = 0;
                            countOutside = 0;
                            dayCountOutside++;
                        }



                    }
                    if (month != currentMonth)
                    {
                        if(location == "Inne")
                        {
                            double aveMonthTempInside = sumDayTempInside / dayCountInside;
                            double aveMonthHumidityInside = sumDayHumidityInside / dayCountInside;

                            CalculateMoldRisk(aveMonthHumidityInside, aveMonthTempInside);

                            sumDayTempInside = 0;
                            sumDayHumidityInside = 0;
                            dayCountInside = 0;
                        }
                        else if (location == "Ute")
                        {
                            double aveMonthTempOutside = sumDayTempOutside / dayCountOutside;
                            double aveMonthHumidityOutside = sumDayHumidityOutside / dayCountOutside;

                            string moldRisk = CalculateMoldRisk(aveMonthHumidityOutside, aveMonthTempOutside);

                            sumDayTempOutside = 0;
                            sumDayHumidityOutside = 0;
                            dayCountOutside = 0;
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
      
        rowCount++;
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
    static float ExtractTemperature(string line)
    {
        Match match = Regex.Match(line, @"\d+\.\d{1}");

        if (match.Success && float.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float temp))
        {
            return temp;
        }

        return 0;
    }

    static int ExtractHumidity(string line)
    {
        Match match = Regex.Match(line, @"\d{2}$");
        return match.Success ? int.Parse(match.Value) : 0;
    }
    static float ExtractAvarageTemp(string line)
    {

    }

}
}