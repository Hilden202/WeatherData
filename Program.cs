namespace WeatherData;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Data.ReadFile.ReadAllWithIgnore("tempdata5-med fel.txt");
            Data.ReadFile.ReadAllFromIgnoreToIndoor("ignore2016_05-2017_01.txt");
            Data.ReadFile.SearchIndoorTemperatureByDate("InneTemperaturer.txt");
            Console.ReadLine();
        }
    }
}

