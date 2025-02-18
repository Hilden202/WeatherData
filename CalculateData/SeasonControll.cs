using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherData.CalculateData
{
    internal class SeasonControll
    {
        public static string SeasonStarted(List<Data.WeatherData> weatherList, string season)
        {
            double temperatureThreshold = season == "Höst" ? 10.0 : 0.0; // (season == "Höst" || season == "Vinter") ? 10.0 : 0.0; //
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
                        if (season == "Höst" && weather.Date.Month >= 8 && weather.Date.Month <= 12 || season == "Vinter")
                        {
                            if (weather.AveTemp < temperatureThreshold)
                            {
                                daysOfSeason++;
                                if (daysOfSeason > highestConsecutive)
                                {
                                    highestConsecutive = daysOfSeason;
                                    closestDay = weather.Date;
                                }
                                if (daysOfSeason == 5)
                                {
                                    string seasonalShift = $"{weather.Date}: det har nu varit 5 dagar av medeltemperatur under {temperatureThreshold.ToString().TemperatureString()}, {season} började {weather.Date.AddDays(-4)}";
                                    return seasonalShift;
                                }
                            }
                            else
                            {
                                daysOfSeason = 0;
                            }
                        }
                    }
                    string seasonChange = $"{season} började aldrig, närmast var {closestDay.AddDays(-highestConsecutive)} med {highestConsecutive} efterföljande dagar där medeltemperaturen låg under {temperatureThreshold.ToString().TemperatureString()}";
                    return seasonChange;
                }
            }
            return ""; // Blank sträng om den inte får fram något annat
        }
    }
}