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
                //Console.WriteLine(total);
                //Console.WriteLine(items);
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
                    //onsole.WriteLine(forecastData.list[i].weather[0].description);
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
            string returnData;
            returnData = string.Format("The current temperature is {0:f1} Degrees C,  and the weather is {1}, the windspeed is {2}, and sunset is {3:hh}:{3:mm}.", oWMCurrent.main.temp,
                oWMCurrent.weather[0].main,oWMCurrent.wind.speed,UnixTimeStampToDateTime(oWMCurrent.sys.sunset));
            return returnData;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}
