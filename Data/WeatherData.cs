using System.Text.RegularExpressions;

namespace WeatherData.Data
{

    public class WeatherData
    {
        public double AveTemp { get; set; }
        public double AveHumidity { get; set; }
        public double RiskOfMold { get; set; }
        public string RiskStatus { get; set; }
        public DateOnly Date { get; set; }
        public string Location { get; set; }

        public WeatherData(double aveTemp, double aveHumidity, double riskOfMold, string riskStatus, DateOnly date, string location)
        {
            AveTemp = aveTemp;
            AveHumidity = aveHumidity;
            RiskOfMold = riskOfMold;
            RiskStatus = riskStatus;
            Date = date;
            Location = location;
        }

        public static List<WeatherData> WeatherList { get; set; } = new List<WeatherData>();

        public void AddWeatherData(double aveTemp, double aveHumidity, double riskOfMold, string riskStatus, DateOnly date, string location)
        {
            WeatherList.Add(new WeatherData(aveTemp, aveHumidity, riskOfMold, riskStatus, date, location));
        }

        public class WeatherDataManager
        {
            public List<WeatherData> WeatherList { get; private set; } = new List<WeatherData>();

            public void AddWeatherData(double aveTemp, double aveHumidity, double riskOfMold, string riskStatus, DateOnly date, string location)
            {
                WeatherList.Add(new WeatherData(aveTemp, aveHumidity, riskOfMold, riskStatus, date, location));
            }

            public static DateOnly ConvertToDateOnly(string dateString)
            {
                var match = Regex.Match(dateString, @"(\d{4})-(\d{2})-(\d{2})");
                if (match.Success)
                {
                    int year = int.Parse(match.Groups[1].Value);
                    int month = int.Parse(match.Groups[2].Value);
                    int day = int.Parse(match.Groups[3].Value);

                    if(month== 12)
                    { }

                    if (month < 1 || month >= 13)
                    {
                        throw new ArgumentException("Exception month format. 1-12");
                    }
                    else if (day < 1 || day > DateTime.DaysInMonth(year, month))
                    {
                        throw new ArgumentException("Exception day format. 1-31");
                    }
                    else
                    {
                        return new DateOnly(year, month, day);
                    }
                }
                throw new ArgumentException("Invalid date format. Expected yyyy-MM-dd");
            }
        }
    }
}