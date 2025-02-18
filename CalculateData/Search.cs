using WeatherData.Data;

namespace WeatherData.CalculateData
{
    internal class Search
    {

        public static void SearchWeatherData(List<Data.WeatherData> weatherList, bool includeDay)
        {
            int year, month, day = 1; // Default day = 1 in case user only wants year + month

            Console.Clear();
            Console.WriteLine("Enter Year (2016-2017): ");

            while (!int.TryParse(Console.ReadLine(), out year) || year < 2016 || year > 2017)
            {
                Console.WriteLine("Invalid input! Enter a valid Year (2016-2017): ");
            }
            Console.Clear();

            Console.WriteLine("Enter Month (1-12): ");

            while (!int.TryParse(Console.ReadLine(), out month) || month < 1 || month > 12)
            {
                Console.WriteLine("Invalid input! Enter a valid Month (1-12): ");
            }

            if (includeDay) // If user wants to specify a day
            {
                Console.Clear();

                Console.WriteLine($"Enter Day (1-{DateTime.DaysInMonth(year, month)}): ");

                while (!int.TryParse(Console.ReadLine(), out day) || day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    Console.WriteLine($"Invalid input! Enter a valid Day (1-{DateTime.DaysInMonth(year, month)}): ");
                }
            }

            DateOnly searchDate = new DateOnly(year, month, day);
            List<Data.WeatherData> results;
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
                string location = " ";
                Console.WriteLine("Vad för infomation vill du ha [U]te eller[I]nne?");

                ConsoleKeyInfo key = Console.ReadKey(true);
                location = key.Key switch
                {
                    ConsoleKey.U => "Ute",
                    ConsoleKey.I => "Inne",
                    _ => string.Empty
                };
                Console.Clear();
                Console.WriteLine("Vad vill du sortera efter?");
                Console.WriteLine("T. Temperatur");
                Console.WriteLine("H. Luftfuktighet");
                Console.WriteLine("M. Mögelrisk");

                key = Console.ReadKey(true);
                {
                    results = key.Key switch
                    {
                        ConsoleKey.T => results.OrderByDescending(p => p.AveTemp).ToList(),
                        ConsoleKey.H => results.OrderBy(p => p.AveHumidity).ToList(),
                        ConsoleKey.M => results.OrderByDescending(p => p.RiskOfMold).ToList(),
                        _ => results
                    };

                    Console.Clear();
                    foreach (var data in results)
                    {
                        if (location == string.Empty || data.Location == location) // Show all if location is invalid
                        {
                            Console.WriteLine($"Date: {data.Date}   ||   Location: {data.Location}");
                            Console.ForegroundColor = data.AveTemp.GetTemperatureColor();
                            Console.WriteLine("Temp: " + data.AveTemp.ToString("F2").TemperatureString());
                            Console.ResetColor();
                            Console.WriteLine("Humidity: " + data.AveHumidity.ToString("F2").ProcentString());
                            Console.WriteLine("Risk: " + data.RiskStatus);
                            Console.WriteLine();
                        }
                    }
                }
                Console.ReadKey();
            }
        }
    }
}