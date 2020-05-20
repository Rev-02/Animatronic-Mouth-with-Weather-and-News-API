using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace ttsApp
{





    public class Coord
    {
        public string lon { get; set; }
        public string lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class CurrentWeather
    {
        public Coord coord { get; set; }
        public IList<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

        public void hello()
        {
            Console.WriteLine("hello");
        }
    }


    public class ForecastMain
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class ForecastWeather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class ForecastWind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class ForecastSys
    {
        public string pod { get; set; }
    }

    public class FcList
    {
        public int dt { get; set; }
        public ForecastMain main { get; set; }
        public IList<ForecastWeather> weather { get; set; }
        public Clouds clouds { get; set; }
        public ForecastWind wind { get; set; }
        public ForecastSys sys { get; set; }
        public string dt_txt { get; set; }
    }


    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
        public int timezone { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class ForecastData
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public IList<FcList> list { get; set; }
        public City city { get; set; }
    }



    class OpenWeatherAPI
    {
        protected const string BASE = "http://api.openweathermap.org";
        protected string Key { get; set; }

        public OpenWeatherAPI(string apikey)
        {
            Key = apikey;
        }
    }
    class OWMCurrent : OpenWeatherAPI
    {
        private const string EXTENSION = "/data/2.5/weather";

        public OWMCurrent(string apikey) : base(apikey)
        {

        }

        public CurrentWeather GetCurrent(string Longitude, string Latitude)
        {
            string urlParameters = "?lon=" + Longitude + "&lat=" + Latitude + "&units=metric" + "&appid=" + Key;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE + EXTENSION);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var returnedData = response.Content.ReadAsAsync<CurrentWeather>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                client.Dispose();
                return returnedData;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
                Exception newEx = new Exception((int)response.StatusCode + " : " + response.ReasonPhrase);
                throw newEx;
            }


        }

        public CurrentWeather GetCurrent(string postcode, string countrycode, string cityname, int mode)
        {
            string urlParameters;
            if (mode == 1)
            {
                urlParameters = "?q=" + postcode + "," + countrycode + "&units=metric" + "&appid=" + Key;
            }
            else
            {
                urlParameters = "?q=" + cityname + "," + countrycode + "&units=metric"+"&appid=" + Key ;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE + EXTENSION);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var returnedData = response.Content.ReadAsAsync<CurrentWeather>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                client.Dispose();
                return returnedData;
            }
            else if ((int)response.StatusCode == 404 && mode == 1)
            {
                return GetCurrent(postcode, countrycode, cityname, 2);
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
                Exception newEx = new Exception((int)response.StatusCode + " : " + response.ReasonPhrase);
                throw newEx;
            }
        }


    }

    class OWMForecast : OpenWeatherAPI
    {
        private const string EXTENSION = "/data/2.5/forecast";

        public OWMForecast(string apikey) : base(apikey)
        {

        }

        public ForecastData ForeCastWeahterData(string postcode, string countrycode, string cityname, int mode)
        {
            string urlParameters;
            if (mode == 1)
            {
                urlParameters = "?q=" + postcode + "," + countrycode + "&appid=" + Key;
            }
            else
            {
                urlParameters = "?q=" + cityname + "," + countrycode + "&appid=" + Key;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE + EXTENSION);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var returnedData = response.Content.ReadAsAsync<ForecastData>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                client.Dispose();
                return returnedData;
            }
            else if ((int)response.StatusCode == 404 && mode == 1)
            {
                return ForeCastWeahterData(postcode, countrycode, cityname, 2);
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
                Exception newEx = new Exception((int)response.StatusCode + " : " + response.ReasonPhrase);
                throw newEx;
            }
        }

        public ForecastData ForeCastWeatherData(string Longitude, string Latitude)
        {
            string urlParameters = "?lon=" + Longitude + "&lat=" + Latitude + "&appid=" + Key;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE + EXTENSION);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var returnedData = response.Content.ReadAsAsync<ForecastData>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                client.Dispose();
                return returnedData;
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
                Exception newEx = new Exception((int)response.StatusCode + " : " + response.ReasonPhrase);
                throw newEx;
            }


        }
    }
}   
