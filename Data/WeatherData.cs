using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    public class WeatherData
    {
        public double AveTemp { get; set; }
        public double AveHumidity { get; set; }
        public string RiskOfMold { get; set; }
        public DateOnly Date { get; set; }
        public string Location { get; set; }

        public WeatherData(double aveTemp, double aveHumidity, string riskOfMold, DateOnly date, string location)
        {
            AveTemp = aveTemp;
            AveHumidity = aveHumidity;
            RiskOfMold = riskOfMold;
            Date = date;
            Location = location;
        }

        public List<WeatherData> WeatherList { get; set; } = new List<WeatherData>();

        public void AddWeatherData(double aveTemp, double aveHumidity, string riskOfMold, DateOnly date, string location)
        {
            WeatherList.Add(new WeatherData(aveTemp, aveHumidity, riskOfMold, date, location));
        }

        public void SortWeatherData(string sortBy)
        {
            if (sortBy.ToLower() == "temperature")
            {
                WeatherList = WeatherList.OrderBy(x => x.AveTemp).ToList();
            }
            else if (sortBy.ToLower() == "humidity")
            {
                WeatherList = WeatherList.OrderBy(x => x.AveHumidity).ToList();
            }
            else if (sortBy.ToLower() == "riskofmold")
            {
                WeatherList = WeatherList.OrderBy(x => x.RiskOfMold).ToList();
            }
            else if (sortBy.ToLower() == "date")
            {
                WeatherList = WeatherList.OrderBy(x => x.Date).ToList();
            }
            else if (sortBy.ToLower() == "location")
            {
                WeatherList = WeatherList.OrderBy(x => x.Location).ToList();
            }
        }
        public static DateOnly ConvertToDateOnly(string dateString)
        {
            var match = Regex.Match(dateString, @"(\d{4})-(\d{2})-(\d{2})");
            if (match.Success)
            {
                int year = int.Parse(match.Groups[1].Value);
                int month = int.Parse(match.Groups[2].Value);
                int day = int.Parse(match.Groups[3].Value);

                if (month < 1 || month > 12)
                {
                    throw new ArgumentException("Exception month format. 1-12");
                }
                else if (day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    throw new ArgumentException("Exception day format. 1-31");
                }
                else
                {
                    return new DateOnly(year, month, day);
                }
            }
            throw new ArgumentException("Invalid date format. Expected yyyy-MM-dd");
        }

        public static void WriteDataInfo(List<WeatherData> weatherList)
        {
            foreach (var info in weatherList)
            {
                Console.WriteLine("Date: " + info.Date + ", Location: " + info.Location);
                Console.WriteLine("Average temperature: " + info.AveTemp + "°C");
                Console.WriteLine("Average hidraition: " + info.AveHumidity);
                Console.WriteLine("Risk of Mold: " + info.RiskOfMold);
                Console.WriteLine("");
            }
        }

        public static List<WeatherData> SearchWeatherData(List<WeatherData> weatherList, bool includeDay)
        {
            int year, month, day = 1; // Default day = 1 in case user only wants year + month

            Console.Clear();
            Console.SetCursorPosition(85, 8);
            Console.WriteLine("Enter Year (2016-2017): ");
            Console.SetCursorPosition(76, 8);

            while (!int.TryParse(Console.ReadLine(), out year) || year < 2016 || year > 2017)
            {
                Console.SetCursorPosition(55, 9);
                Console.WriteLine("Invalid input! Enter a valid Year (2016-2017): ");
            }

            Console.SetCursorPosition(101, 8);
            Console.WriteLine("Enter Month (1-12): ");
            Console.SetCursorPosition(101, 9);

            while (!int.TryParse(Console.ReadLine(), out month) || month < 1 || month > 12)
            {
                Console.SetCursorPosition(55, 10);
                Console.WriteLine("Invalid input! Enter a valid Month (1-12): ");
            }

            if (includeDay) // If user wants to specify a day
            {
                Console.SetCursorPosition(115, 8);
                Console.WriteLine($"Enter Day (1-{DateTime.DaysInMonth(year, month)}): ");
                Console.SetCursorPosition(115, 9);

                while (!int.TryParse(Console.ReadLine(), out day) || day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    Console.SetCursorPosition(55, 10);
                    Console.WriteLine($"Invalid input! Enter a valid Day (1-{DateTime.DaysInMonth(year, month)}): ");
                }
            }

            DateOnly searchDate = new DateOnly(year, month, day);

            // Search in list
            List<WeatherData> results;
            if (includeDay)
            {
                results = weatherList.Where(w => w.Date == searchDate).ToList(); // Exact match
            }
            else
            {
                results = weatherList.Where(w => w.Date.Year == year && w.Date.Month == month).ToList(); // All from month
            }

            if (results.Count == 0)
            {
                Console.WriteLine("No matching records found.");
            }
            else
            {
                foreach (var data in results)
                {
                    Console.WriteLine($"Date: {data.Date}, Temp: {data.AveTemp}, Humidity: {data.AveHumidity}, Risk: {data.RiskOfMold}, Location: {data.Location}");
                    Console.WriteLine();
                }
            }

            return results;
        }
    }
}
