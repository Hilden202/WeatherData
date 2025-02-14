namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        var weatherList = new List<WeatherData>();
        Data.ReadFile.ReadAllTest("../../../Files/Tempdata.txt", weatherList);


        Data.ReadFile.ReadAllAndCreateFileTempdata("tempdata5-med fel.txt"); // Skapar en koreekt fil med temperaturdata Tempdata.txt
        Data.ReadFile.CreateFileIndoor("Tempdata.txt"); // Skapar en fil med innetemperaturer från Tempdata.txt
        Data.ReadFile.CreateFileOutdoor("Tempdata.txt"); // Skapar en fil med utetemperaturer från Tempdata.txt
        Data.ReadFile.CreateFileAverageTempOutside("UteTemperaturer.txt"); // Skapar en fil AvergageTemperaturUte.txt med medelvärde från UteTemperaturer.txt
        Data.ReadFile.CreateFileAverageHumOutside("UteTemperaturer.txt"); // Skapar en fil AvergageFuktUte.txt med medelvärde av UteTemperaturer.txt
                                                                           //Data.ReadFile.CreateFileAverageTempInside("InneTemperaturer.txt"); // Skapar en fil med medelvärde av InneTemperaturer.
                                                                           //Data.ReadFile.CreateFileAverageHumInside("InneTemperaturer.txt"); // Skapar en fil med medelvärde av InneTemperaturer.txt


        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("T. Sök utetemperatur och luftfuktighet för ett datum.");
            Console.WriteLine("I. Sök medel innertemperatur för ett datum.");
            Console.WriteLine("S. Sök sorterad temperaturlista.");
            Console.WriteLine("M. Meterologisk höst/vinter.");
            Console.WriteLine("Q. Avsluta.");

            var key = Console.ReadKey(true);


            switch (key.Key)
            {
                case ConsoleKey.T:
    {
        Console.Clear();
        bool includeDay = true;

        List<WeatherData> monthResults = WeatherData.SearchWeatherData(weatherList, true);

        break;
    }
case ConsoleKey.I:
    {
        Console.Clear();
        bool includeDay = false;

        List<WeatherData> monthResults = WeatherData.SearchWeatherData(weatherList, false);

        break;
    }
                case ConsoleKey.S:
                    Console.Clear();
                    Console.WriteLine("Välj ett alternativ:");
                    Console.WriteLine("1. Sortera temperaturer i fallande ordning.");
                    Console.WriteLine("2. Sortera luftfuktighet i stigande ordning.");

                    var key2 = Console.ReadKey(true);

                    switch (key2.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();
                            Data.ReadFile.SortDataBy("AverageTemperaturerUte.txt", true);
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Data.ReadFile.SortDataBy("AverageFuktUte.txt", false);
                            break;
                    }
                    break;
                case ConsoleKey.M:
                    Console.Clear();
                    Console.WriteLine("Välj ett alternativ:");
                    Console.WriteLine("1. Meterologisk höst.");
                    Console.WriteLine("2. Meterologisk vinter.");

                    var key3 = Console.ReadKey(true);

                    switch (key3.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();
                            Data.ReadFile.SeasonStarted("AverageTemperaturerUte.txt", "Höst");
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Data.ReadFile.SeasonStarted("AverageTemperaturerUte.txt", "Vinter");
                            break;
                    }
                    break;
                case ConsoleKey.Q:
                    return;
                default:
                    Console.WriteLine("\nOgiltigt val, försök igen.");
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Tryck på en tangent för att återgå till menyn.");
            Console.ReadKey(true);
        }
    }
}