namespace WeatherData.Data
{
    internal interface ISortedData
    {
        void AddData(string date, double temperature, double humidity, string riskOfMold, string location);
        //List<(string Date, double Temperature, double Humidity, string RiskOfMold)> GetSortedMonth(string month, string sortBy, string location);
        List<(string Date, double Temperature, double Humidity, string RiskOfMold)> GetSortedDay(string day, string sortBy, string location);
    }
    public class SortedData : ISortedData
    {
    //    private Dictionary<string, List<(string Date, double Temperature, double Humidity, string RiskOfMold)>> monthlyInsideData;
    //    private Dictionary<string, List<(string Date, double Temperature, double Humidity, string RiskOfMold)>> monthlyOutsideData;
        private Dictionary<string, List<(string Date, double Temperature, double Humidity, string RiskOfMold)>> dailyInsideData;
        private Dictionary<string, List<(string Date, double Temperature, double Humidity, string RiskOfMold)>> dailyOutsideData;

        public SortedData()
        {
            //monthlyInsideData = new Dictionary<string, List<(string, double, double, string)>>();
            //monthlyOutsideData = new Dictionary<string, List<(string, double, double, string)>>();
            dailyInsideData = new Dictionary<string, List<(string, double, double, string)>>();
            dailyOutsideData = new Dictionary<string, List<(string, double, double, string)>>();
        }




    public void AddData(string date, double temperature, double humidity, string riskOfMold, string location)
    {
        string monthKey = date.Substring(0, 7); // Extract YYYY-MM (Month key)
        string dayKey = date; // Use full date as key

        if (location.ToLower() == "inside")
        {
            //if (!monthlyInsideData.ContainsKey(monthKey))
            //    monthlyInsideData[monthKey] = new List<(string, double, double, string)>();
            //monthlyInsideData[monthKey].Add((date, temperature, humidity, riskOfMold));

            if (!dailyInsideData.ContainsKey(dayKey))
                dailyInsideData[dayKey] = new List<(string, double, double, string)>();
            dailyInsideData[dayKey].Add((date, temperature, humidity, riskOfMold));
        }
        else if (location.ToLower() == "outside")
        {
            //if (!monthlyOutsideData.ContainsKey(monthKey))
            //    monthlyOutsideData[monthKey] = new List<(string, double, double, string)>();
            //monthlyOutsideData[monthKey].Add((date, temperature, humidity, riskOfMold));

            if (!dailyOutsideData.ContainsKey(dayKey))
                dailyOutsideData[dayKey] = new List<(string, double, double, string)>();
            dailyOutsideData[dayKey].Add((date, temperature, humidity, riskOfMold));
        }
    }


    //public List<(string Date, double Temperature, double Humidity, string RiskOfMold)> GetSortedMonth(string month, string sortBy, string location)
    //    {
    //        Dictionary<string, List<(string, double, double, string)>> choosenData =
    //            location.ToLower() == "inside" ? monthlyInsideData : monthlyOutsideData;

    //        if (!choosenData.ContainsKey(month))
    //        {
    //            return new List<(string, double, double, string)>();
    //        }
    //        else if (sortBy.ToLower() == "temperature")
    //        {
    //            return choosenData[month].OrderBy(entry => entry.Item2).ToList();
    //        }
    //        else if (sortBy.ToLower() == "humidity")
    //        {
    //            return choosenData[month].OrderBy(entry => entry.Item3).ToList();
    //        }
    //        else
    //        {
    //            return choosenData[month].OrderBy(entry => entry.Item4).ToList();
    //        }
    //    }
        public List<(string Date, double Temperature, double Humidity, string RiskOfMold)> GetSortedDay(string day, string sortBy, string location)
        {
            Dictionary<string, List<(string, double, double, string)>> choosenData =
                location.ToLower() == "inside" ? dailyInsideData : dailyOutsideData;

            if (!choosenData.ContainsKey(day))
            {
                return new List<(string, double, double, string)>();
            }
            else if (sortBy.ToLower() == "temperature")
            {
                return choosenData[day].OrderBy(entry => entry.Item2).ToList();
            }
            else if (sortBy.ToLower() == "humidity")
            {
                return choosenData[day].OrderBy(entry => entry.Item3).ToList();
            }
            else
            {
                return choosenData[day].OrderBy(entry => entry.Item4).ToList();
            }
        }
    }
}

