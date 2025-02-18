using WeatherData.Data;
namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        var weatherManager = ReadFile.ReadWeatherInfo("../../../Files/tempdata5-med fel.txt");
        // function som skriver ut till fill
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
                    Helpers.SearchWeatherData(weatherManager, true);
                    break;
                }
            case ConsoleKey.I:
                {
                    bool includeDay = false;
                    Helpers.SearchWeatherData(weatherManager, false);
                    break;
                }
            case ConsoleKey.M:
                {
                    string season = "Höst";


                    Console.WriteLine(SeasonControll.SeasonStarted(weatherManager, "Höst"));
                    Console.WriteLine(SeasonControll.SeasonStarted(weatherManager, "Vinter"));
                    break;
                }

        }


    }
}

