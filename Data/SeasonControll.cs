using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherData.Data
{
    internal class SeasonControll
    {
        public static void SeasonStarted(List<Data.WeatherData> weatherList, string season)
        {
            



            double temperatureThreshold = (season == "Höst" ? 10.0 : 0.0); // (season == "Höst" || season == "Vinter") ? 10.0 : 0.0; //
            DateOnly closestDay = new DateOnly();
            int daysOfSeason = 0;
            int highestConsecutive = 0;

            var result = weatherList.GroupBy(x => x.Location).ToList();
            

            for (int i = 0; i < result.Count; i++)
            {
                var location = result[i].Key;
                if (location == "Ute")
                {
                    foreach (var weather in result[i])
                    {
                        if ((season == "Höst" && weather.Date.Month >= 8) && weather.Date.Month <= 12 || season == "Vinter")
                        {
                            if (weather.AveTemp < temperatureThreshold)
                            {
                                daysOfSeason++;
                                //Console.WriteLine(weather.Date + "\t" + weather.AveTemp.ToString("F2"));
                                //Console.WriteLine(daysOfSeason);
                                if (daysOfSeason > highestConsecutive)
                                {
                                    highestConsecutive = daysOfSeason;
                                    closestDay = weather.Date;
                                }
                                if (daysOfSeason == 5)
                                {
                                    Console.WriteLine($"{weather.Date}: det har nu varit 5 dagar av medeltemperatur under {temperatureThreshold}");
                                    Console.WriteLine($"{season} började " + weather.Date.AddDays(-4));
                                    return;
                                }
                            }
                            else
                            {
                                //Console.WriteLine(weather.Date + " nollställd" + weather.AveTemp.ToString("F2") + "   " + weather.Location);
                                daysOfSeason = 0;
                            }
                        }

                    }
                    Console.WriteLine($"{season} började aldrig, närmast var {closestDay} efter {highestConsecutive} dagar av medeltemperatur under {temperatureThreshold}");


                }
                //else
                //{
                    
                //    //Console.WriteLine(weather.Date + " Matchade ej season date");
                //}
            }
        }
           
    }
}
