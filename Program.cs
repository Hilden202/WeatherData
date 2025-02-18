using WeatherData.CalculateData;
using WeatherData.Data;
namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        var weatherManager = ReadFile.ReadWeatherInfo("../../../Files/tempdata5-med fel.txt");

        InfoToText.WriteInfo.WriteTextFile(weatherManager);
        bool run = true;
        while (run == true)
        {
            Console.Clear();
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("T. Sök specific dag.");
            Console.WriteLine("I. Sök specific månad.");
            Console.WriteLine("M. Meterologisk höst/vinter.");
            Console.WriteLine("Q. Avsluta.");

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.T:
                    {
                        bool includeDay = true;
                        Search.SearchWeatherData(weatherManager, true);
                        break;
                    }
                case ConsoleKey.I:
                    {
                        bool includeDay = false;
                        Search.SearchWeatherData(weatherManager, false);
                        break;
                    }
                case ConsoleKey.M:
                    {
                        Console.Clear();
                        Console.WriteLine(SeasonControll.SeasonStarted(weatherManager, "Höst"));
                        Console.WriteLine(SeasonControll.SeasonStarted(weatherManager, "Vinter"));
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        run = false;
                        break;
                    }
            }
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri tangent för att gå tillbaka till menyn...");
            Console.ReadKey(true);
        }
    }
}