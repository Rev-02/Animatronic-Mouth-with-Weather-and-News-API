using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.IO.Ports;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Globalization;
using System.Windows.Input;

namespace ttsApp
{
    class Program
    {

        static void Main(string [] args)
        {
            ParameterizedThreadStart Eyes = new ParameterizedThreadStart(MainLoop);
            Mouth m = new Mouth("Microsoft David Desktop");
            // start them  
            Thread Eyethread = new Thread(Eyes);
            //Eyethread.Start(m);
            FaceController faceController = new FaceController("COM22", 115200);
            faceController.POST();
            Console.ReadKey();
            
        }

        static void MainLoop(object mouth)
        {
            Mouth m = (Mouth)mouth;
            reader keyreader = new reader();
            string[] keys = keyreader.ReadKeys();
            
            NewsApiTop newsAPI = new NewsApiTop(keys[0]);
            OWMForecast oWMForecast = new OWMForecast(keys[1]);
            OWMCurrent oWM = new OWMCurrent(keys[1]);
            Interpreter interpreter = new Interpreter();
            string intext;
            while (true)
            {
                Console.Write("Press 1 for Full Update, 2 for news, 3 for weather \t");
                intext = Console.ReadLine();
                switch (intext)
                {
                    default:
                        break;
                    case "1":
                        speakNews(m, interpreter, newsAPI);
                        speakWeather(m, interpreter, oWMForecast, oWM);
                        break;
                    case "2":
                        speakNews(m, interpreter, newsAPI);

                        break;
                    case "3":

                        speakWeather(m, interpreter, oWMForecast, oWM);
                        break;

                }
            }
        }

        private static void speakNews(Mouth mouth, Interpreter interpreter, NewsApiTop newsAPI)
        {
            TopNews data = newsAPI.GetTopNews("gb");
            mouth.speakMsg(string.Format("The top 5 news stories today are:"));
            foreach (string story in interpreter.Top5(data))
            {
                mouth.speakMsg(string.Format(story));
            }
        }

        private static void speakWeather (Mouth mouth, Interpreter interpreter, OWMForecast oWMForecast, OWMCurrent oWM)
        {
            
            ForecastData fc = oWMForecast.ForeCastWeahterData("cv5", "GB", "Coventry", 1);
            
            List<DateTime> days = new List<DateTime>();
            days = interpreter.containsDays(fc);

            
            try
            {
                var returned = oWM.GetCurrent("cv5", "GB", "Coventry", 1);
                mouth.speakMsg(string.Format(interpreter.CurrentSummary(returned)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            DateTime nowtime = DateTime.Now;
            bool today = false;
            foreach (var day in days)
            {
                if (day.Day == nowtime.Day)
                {
                    today = true;
                    mouth.speakMsg(string.Format("The average temperature today will be {0:f1} Degrees C", interpreter.CalcAverageForecastTemp(fc, day)));
                    mouth.speakMsg(string.Format("The forecast for later today will be {0} .", interpreter.CalcForecastMain(fc, day)));
                }
            }
            if (today)
            {
                mouth.speakMsg(string.Format("The weather for tommorow is {0}", interpreter.CalcForecastMain(fc, days[1])));
                mouth.speakMsg(string.Format("The average temperature tomorrow will be {0:f1} Degrees C", interpreter.CalcAverageForecastTemp(fc, days[1])));
            }
            else
            {
                mouth.speakMsg(string.Format("The weather for {1:dddd} is {0}", interpreter.CalcAverageForecastTemp(fc, days[0]), days[0]));
                mouth.speakMsg(string.Format("The average temperature for {1:dddd} will be {0:f1} Degrees C", interpreter.CalcAverageForecastTemp(fc, days[0]), days[0]));
            }
        }

        static void processEyes(object mouth)
        {
            Console.WriteLine("Hi");

        }


        
    }
}
