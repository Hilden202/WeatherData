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
            input = input + "C°";
            return input;
        }
    }
}
