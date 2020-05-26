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
using System.ComponentModel.Design;
using System.Diagnostics;

namespace ttsApp
{
    class Program
    {
        private static Random random; 
        private static FaceController faceController;
        private static Mouth m;
        private static EyeController eyeController;
        private static ReaderWriterLockSlim mouthlock = new ReaderWriterLockSlim();
        private static ReaderWriterLockSlim eyelock = new ReaderWriterLockSlim();
        private static int[][] Eyes = new int[2][];
        private static string PortQueue = "0"; 
        static void Main(string [] args)
        {
            random = new Random();
            faceController = new FaceController("COM4", 115200);
            m = new Mouth("Microsoft David Desktop");
            eyeController = new EyeController();

            ThreadStart eyethread = new ThreadStart(processEyes);
            ThreadStart portwriter = new ThreadStart(writeData);
            ThreadStart main = new ThreadStart(MainLoop);
            Eyes = eyeController.Eyes;
            m.MouthPosChanged += mouthEventHandler;
            eyeController.EyesChanged += writeEyevals;
            
            faceController.POST();

            // start them  
            
            Thread writerThread = new Thread(portwriter);
            Thread interfaceThread = new Thread(main);
            Thread Eyethread = new Thread(eyethread);
            
            writerThread.Start();
            Thread.Sleep(3000);
            interfaceThread.Start();
            Eyethread.Start();
            
        }

        static void MainLoop()
        {
            
            reader keyreader = new reader();
            string[] keys = keyreader.ReadKeys();
            
            NewsApiTop newsAPI = new NewsApiTop(keys[0]);
            OWMForecast oWMForecast = new OWMForecast(keys[1]);
            OWMCurrent oWM = new OWMCurrent(keys[1]);
            Interpreter interpreter = new Interpreter();
            
            while (true)
            {
                string intext = "";
                Console.WriteLine("Press 1 for Full Update, 2 for news, 3 for weather");
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
                mouth.speakMsg(string.Format("The weather for {1:dddd} is {0}", interpreter.CalcForecastMain(fc, days[0]), days[0]));
                mouth.speakMsg(string.Format("The average temperature for {1:dddd} will be {0:f1} Degrees C", interpreter.CalcAverageForecastTemp(fc, days[0]), days[0]));
            }
        }

        static void processEyes()
        {

            
            while (true)
            {
                eyeController.blink();
                if(random.Next(5) == 2)
                {
                    eyeController.blink();
                    if(random.Next(10) == 10)
                    {
                        eyeController.blink();
                    }
                }
                Thread.Sleep(random.Next(5000));
                Thread.Sleep(1000);
                //eyeController.disco();
            }
        }

        static void writeData()
        {
            while (true)
            {
                eyelock.EnterReadLock();
                mouthlock.EnterReadLock();
                try
                {
                    
                    faceController.writeFace(PortQueue, Eyes);
                    
                }
                finally
                {
                    eyelock.ExitReadLock();
                    mouthlock.ExitReadLock();
                    Thread.Sleep(5);
                }
                
            }
        }

        public static void mouthEventHandler(object sender, MouthPosChangedEventArgs e)
        {
            
            mouthlock.EnterWriteLock();
            try
            {
                
                PortQueue = e.Pos;
            }
            finally
            {
                mouthlock.ExitWriteLock();
            }
            
        }

        public static void writeEyevals(object sender, EyesChangedEventArgs e)
        {
            eyelock.EnterWriteLock();
            try
            {
                Eyes = e.Eyes;

            }
            catch
            {
                Console.WriteLine("Escaped");
            }
            finally
            {
                
                eyelock.ExitWriteLock();
            }

        }

        

    }
}
