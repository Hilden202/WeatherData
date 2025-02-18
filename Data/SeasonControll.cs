using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    internal class SeasonControll
    {
        public static void SeasonStarted(List<Data.WeatherData> weatherList, string season)
        {
            

            //double temperatureThreshold = 0.0;
            //if (season == "Höst")
            //{
            //    temperatureThreshold = 10.0;
            //}
            //if (season == "Vinter")
            //{
            //    temperatureThreshold = 0.0;
            //}

            double temperatureThreshold = (season == "Höst" ? 10.0 : 0.0);

            for(int i = 0; i < weatherList.Count; i++)
            {
                var weather = weatherList[i];
                int daysOfSeason = 0;
                int highestConsecutive = 0;
                int closestDay = 0;
                if((season == "höst" && weather.Date.Month >= 8) && weather.Date.Month <= 12 || weather.Date.Month <= 2 && weather.Date.Month >= 1 )
                {

                }
                
            }
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(path + fileName))
            {
                string pattern = @"(\d{4}-\d{2}-\d{2}): (-{0,1}\d{1,2}.\d)";
                string line = reader.ReadLine();
                int daysOfSeason = 0;
                int highestConsecutive = 0;
                int closestDay = 0;
                while (line != null)
                {
                    foreach (Match m in Regex.Matches(line, pattern))
                    {
                        if (m.Success)
                        {
                            list.Add(m.Groups[1].Value.ToString() + " med temperaturen " + m.Groups[2].Value.ToString());
                            DateOnly date = DateOnly.Parse(m.Groups[1].ToString());
                            DateOnly earliestAutumn = new DateOnly(2016, 08, 01);
                            if (double.Parse(m.Groups[2].Value.ToString().Replace('.', ',')) < temperatureThreshold && date.CompareTo(earliestAutumn) >= 0)
                            {
                                daysOfSeason++;
                                if (daysOfSeason > highestConsecutive)
                                {
                                    highestConsecutive = daysOfSeason;
                                    closestDay = list.Count;
                                }
                                if (daysOfSeason == 5)
                                {
                                    Console.WriteLine(date);
                                    Console.WriteLine($"{season} började " + list[list.Count - 5]);
                                    return;
                                }
                            }
                            else
                            {
                                daysOfSeason = 0;
                            }
                        }
                    }
                    line = reader.ReadLine();
                }
                Console.WriteLine($"{season} har inte börjat inom intervallen, närmast var " + list[closestDay]);
            }
        }
    }
}
