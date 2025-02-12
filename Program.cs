namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Data.ReadFile.ReadAllAndCreateFileTempdata("tempdata5-med fel.txt");
            Data.ReadFile.CreateFileIndoor("Tempdata.txt");
            Data.ReadFile.CreateFileOutdoor("Tempdata.txt");

             while (true)
            {
                Console.Clear();
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("T. Sök utetemperatur och luftfuktighet för ett datum.");
                Console.WriteLine("I. Sök medel innertemperatur för ett datum.");
                Console.WriteLine("Q. Avsluta.");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.T:
                        Data.ReadFile.SearchOutdoorTempAndHumidityByDate("UteTemperaturer.txt");
                        break;
                    case ConsoleKey.I:
                        Data.ReadFile.SearchAvarageIndoorTempByDate("InneTemperaturer.txt");
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
}

