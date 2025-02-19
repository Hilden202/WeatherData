using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData.Data
{
    internal static class Extensions
    {
        public static string TemperatureString(this string input)
        {
            input = input + "°C";
            return input;
        }
        public static string ProcentString(this string input)
        {
            input = input + "%";
            return input;
        }

        public static ConsoleColor GetTemperatureColor(this double temp)
        {
            if (temp < 0) return ConsoleColor.Blue;
            if (temp < 10) return ConsoleColor.Cyan;
            if (temp < 17) return ConsoleColor.Green;
            if (temp < 25) return ConsoleColor.Yellow;
            return ConsoleColor.Red;
        }
    }
}