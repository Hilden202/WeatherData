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
            Console.ReadLine();
        }
    }
}

