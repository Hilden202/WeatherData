namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        ReadAllTest();

        weatherData.WriteDataInfo();

            Data.ReadFile.ReadAllAndCreateFileTempdata("tempdata5-med fel.txt"); // Skapar en koreekt fil med temperaturdata Tempdata.txt
            Data.ReadFile.CreateFileIndoor("Tempdata.txt"); // Skapar en fil med innetemperaturer från Tempdata.txt
            Data.ReadFile.CreateFileOutdoor("Tempdata.txt"); // Skapar en fil med utetemperaturer från Tempdata.txt
            Data.ReadFile.CreateFileAverageTempOutside("UteTemperaturer.txt"); // Skapar en fil AvergageTemperaturUte.txt med medelvärde från UteTemperaturer.txt
            //Data.ReadFile.CreateFileAverageTempInside("InneTemperaturer.txt"); // Skapar en fil med medelvärde av InneTemperaturer.
            //Data.ReadFile.CreateFileAverageHumOutside("UteTemperaturer.txt"); // Skapar en fil med medelvärde av UteTemperaturer.txt
            //Data.ReadFile.CreateFileAverageHumInside("InneTemperaturer.txt"); // Skapar en fil med medelvärde av InneTemperaturer.txt

            string temperaturePattern = @"(\d{4}-\d{2}-\d{2}): (-{0,1}\d{1,2},\d)";
            // string temperaturePattern = @"(\d{4}-\d{2}-\d{2}):\s*(-?\d+\.\d+)";
            string humidityPattern = @"(\d{4}-\d{2}-\d{2}).*((?<=,)\d{1,2})";

             while (true)
            {
                Console.Clear();
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("T. Sök utetemperatur och luftfuktighet för ett datum.");
                Console.WriteLine("I. Sök medel innertemperatur för ett datum.");
                Console.WriteLine("S. Sök sorterad temperaturlista.");
                Console.WriteLine("Q. Avsluta.");

                var key = Console.ReadKey(true);


                switch (key.Key)
                {
                    case ConsoleKey.T:
                        Console.Clear();
                        Data.ReadFile.SearchOutdoorTempAndHumidityByDate("UteTemperaturer.txt");
                        break;
                    case ConsoleKey.I:
                        Console.Clear();
                        Data.ReadFile.SearchAvarageIndoorTempByDate("InneTemperaturer.txt");
                        break;
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
                            Data.ReadFile.SortDataBy("AverageTemperaturerUte.txt", temperaturePattern, true);
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            Data.ReadFile.SortDataBy("InneTemperaturer.txt", humidityPattern, false);
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