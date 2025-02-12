using System;
using System.Text.RegularExpressions;

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
                    string line = reader.ReadLine();
                    int rowCount = 0;
                    string currentDate = null;
                    
                    double sumTemperature = 0; // avarage day
                    double sumMoist = 0;

                    double sumDayTemp   //avarage month
                    double sumDayHumidity
                    int count = null;
                
                    

                    while (!reader.EndOfStream)
                    {
                        if (!Regex.IsMatch(line, @"^(2016-(0[6-9]|1[0-2])|2016-05-(3[1])|2017-01-(0[1-9]|10))\s"))
                        {
                            Regex regex = new Regex(@"(\d{4})-(\d{2})-(\d{2})");
                            


                            string line = reader.ReadLine();
                            Match match = regex.Match(line);

                            if (match.Success)
                            {
                                string year = match.Groups[1].Value; // Om vi ska räkna flera år
                                string month = match.Groups[2].Value;
                                string day = match.Groups[3].Value;

                                float temperature = ExtractTemperature(line);
                                int humidity = ExtractHumidity(line);
                            
                                if (currentDate == null)
                                {
                                    currentDate = date;
                                }

                                if (date != currentDate)
                                {
                                    double aveDayTemp = sumTemperature / count;
                                    double aveDayHumidity = humidity / count;

                                    sumDayHumidity += aveDayHumidity;
                                    sumDayTemp += aveDayTemp;

                                    currrentDate = date;
                                    sumTemperature = 0;
                                    sumHumidity = 0;
                                    count = 0;

                                    double moldRisk = (aveDayHumidity - 78) * (aveDayTemp / 15) / 0.22;

                                    string riskLevel = moldRisk > 59 ? "High Risk of mold" : "Low Risk of mold";
                                }
                                
                            sumTemperature += temp;
                            sumHumidity += humidity;
                            count ++;
                        }
                           
                    }
                            
               }
               if (count == 0)
               {
                
               }
                        rowCount++;
                        line = reader.ReadLine();
                    }
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