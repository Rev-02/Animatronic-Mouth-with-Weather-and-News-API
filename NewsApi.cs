using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ttsApp
{
    public class Source
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Article
    {
        public Source source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
    }

    public class TopNews
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public IList<Article> articles { get; set; }
    }

    class NewsAPI
    {
        protected const string BASE = "https://newsapi.org";
        protected string Key { get; set; }

        public NewsAPI(string apikey)
        {
            Key = apikey;
        }
    }

    class NewsApiTop : NewsAPI
    {
        private const string EXTENSION = "/v2/top-headlines";

        public NewsApiTop(string apikey) : base(apikey)
        {

        }

        public TopNews GetTopNews(string countryCode)
        {
            string urlParameters = "?country="+countryCode+"&apiKey="+Key;
            //urlParameters = "?lon=4.55&appid=0acc67f54faacb1b6040ccbbd8f38a3f";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE+EXTENSION);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var returnedData = response.Content.ReadAsAsync<TopNews>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
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

