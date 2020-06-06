using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ttsApp
{
    class Interpreter
    {
        public double CalcAverageForecastTemp(ForecastData forecastData, DateTime date)
        {
            bool flag = false;
            double total = 0;
            int length = forecastData.cnt;
            double items = 0;
            for (int i = 0; i < length - 1; i++)
            {
                if (Convert.ToDateTime(forecastData.list[i].dt_txt).Day == date.Day)
                {
                    if (flag == false)
                    {
                        flag = true;
                    }
                    total += (forecastData.list[i].main.temp);
                    items++;
                }
            }
            if (flag)
            {
                
                return (total / items) -273.15;
            }
            else
            {
                return double.NaN;
            }
        }

        public double CalcAverageForecastTemp(ForecastData forecastData)
        {
            double total = 0;
            int length = forecastData.cnt;
            for (int i = 0; i < length - 1; i++)
            {
                total += forecastData.list[i].main.temp;
            }
            return (total / length) - 273.15;
        }

        public string CalcForecastMain(ForecastData forecastData, DateTime date)
        {
            List<string> commonList = new List<string>();
            string mostCommon;
            bool flag = false;
            int length = forecastData.cnt;
            for (int i = 0; i < length - 1; i++)
            {
                if (Convert.ToDateTime(forecastData.list[i].dt_txt).Day == date.Day)
                {
                    if (flag == false)
                    {
                        flag = true;
                    }
                    //Console.WriteLine(forecastData.list[i].weather[0].description);
                    commonList.Add(forecastData.list[i].weather[0].description);
                }
            }
            if (flag)
            {
                var groupsWithCounts = from s in commonList
                                       group s by s into g
                                       select new
                                       {
                                           Item = g.Key,
                                           Count = g.Count()
                                       };

                var groupsSorted = groupsWithCounts.OrderByDescending(g => g.Count);
                mostCommon = groupsSorted.First().Item;
                return mostCommon;
            }
            else
            {
                return string.Empty;
            }
        }

        public List<DateTime> containsDays(ForecastData forecastData)
        {
            List<int> daylist = new List<int>();
            List<DateTime> returnDays = new List<DateTime>();
            int length = forecastData.cnt;
            for (int i = 0; i < length - 1; i++)
            {
                int day = Convert.ToDateTime(forecastData.list[i].dt_txt).Day;
                if (!daylist.Contains(day))
                {
                    daylist.Add(day);
                    returnDays.Add(Convert.ToDateTime(forecastData.list[i].dt_txt));
                }
            }
            return returnDays;

        }

        public List<string> Top5(TopNews top)
        {
            List<string> returnData = new List<string>();
            int count = 5;
            foreach (Article article in top.articles)
            {
                if (count < 10)
                {
                    returnData.Add(article.title);
                    count++;
                }
                else
                {
                    continue;
                }
            }

            return returnData;
        }

        public string CurrentSummary(CurrentWeather oWMCurrent)
        {
            StringBuilder returnString = new StringBuilder();
            returnString.AppendLine(string.Format("The current temperature is {0:f1} Degrees C, and the weather in {1} is currently:", oWMCurrent.main.temp, AtLocation(oWMCurrent)));
                   
            for (int i = 0; i < oWMCurrent.weather.Count; i++)
            {
                if (i < oWMCurrent.weather.Count - 1)
                {
                    returnString.AppendLine(oWMCurrent.weather[i].description);
                    returnString.AppendLine("With:");
                }
                else
                {
                    returnString.AppendLine(oWMCurrent.weather[i].description);
                }
            }

            if (oWMCurrent.IsRain1h())
            {
                returnString.AppendLine(string.Format(". The volume of rain in the last hour was {0} mm.",oWMCurrent.rain.oneHour));
            }
            if (oWMCurrent.IsSnow1h())
            {
                returnString.AppendLine(string.Format(". The volume of snow in the last hour was {0} mm.",oWMCurrent.snow.oneHour));
            }
            returnString.AppendLine(string.Format("the windspeed is {0} metres per second , and sunset is {1:hh}:{1:mm}.", oWMCurrent.wind.speed, UnixTimeStampToDateTime(oWMCurrent.sys.sunset)));


            return returnString.ToString();
        }

        public string ForecastSummary(ForecastData fc)
        {
            List<DateTime> days = new List<DateTime>();
            days = containsDays(fc);
            DateTime nowtime = DateTime.Now;
            bool today = false;
            StringBuilder returnString = new StringBuilder();
            foreach (var day in days)
            {
                if (day.Day == nowtime.Day)
                {
                    today = true;
                    returnString.AppendLine(string.Format("The average temperature for {1} today will be {0:f1} Degrees C.", CalcAverageForecastTemp(fc, day), AtLocation(fc)));
                    returnString.AppendLine(string.Format("The forecast for later today will be {0} .", CalcForecastMain(fc, day)));
                }
            }
            if (today)
            {
                returnString.AppendLine(string.Format("The weather for {1} tommorow is {0}.", CalcForecastMain(fc, days[1]), AtLocation(fc)));
                returnString.AppendLine(string.Format("The average temperature tomorrow will be {0:f1} Degrees C", CalcAverageForecastTemp(fc, days[1])));
            }
            else
            {
                returnString.AppendLine(string.Format("The weather for {1:dddd} is {0}", CalcForecastMain(fc, days[0]), days[0]));
                returnString.AppendLine(string.Format("The average temperature for {1:dddd} will be {0:f1} Degrees C", CalcAverageForecastTemp(fc, days[0]), days[0]));
            }
            return returnString.ToString();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public string AtLocation(CurrentWeather oWMCurrent)
        {
            return oWMCurrent.name;
        }

        public string AtLocation(ForecastData forecastData)
        {
            return forecastData.city.name;
        }

    }
}
