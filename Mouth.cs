using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ttsApp
{
    class Mouth
    {
        static SerialPort port;
        private int[] positions = new int[21];
        SpeechSynthesizer speak = new SpeechSynthesizer();
        public string voice { get; set;  }

        public  Mouth(string portNum, string Voice)
        {
            voice = Voice;
            speak.SetOutputToDefaultAudioDevice();
            speak.VisemeReached += new EventHandler<VisemeReachedEventArgs>(synthVisemeReached);
            reader rdr = new reader();
            positions = rdr.ReadFile();
            try
            {
                port = new SerialPort(portNum, 115200);
                port.Open();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                
            }
        }

        public void POST()
        {
            try
            {
                port.WriteLine("120");
                Thread.Sleep(1500);
                port.WriteLine("0");

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            
        }

        public void speakMsg(string Message)
        {
            PromptBuilder speakRate = new PromptBuilder();
            speakRate.StartVoice(voice);
            speakRate.AppendText(Message, PromptRate.Slow);
            speakRate.EndVoice();
            speak.Speak(speakRate);
            speakRate.ClearContent();
        }

        public void synthVisemeReached(object sender, VisemeReachedEventArgs e)
        {
            port.WriteLine(Convert.ToString(positions[Convert.ToInt32(e.Viseme)]));
            //Console.WriteLine(e.Viseme);
            /*if (port.IsOpen)
            {

                port.WriteLine(Convert.ToString(positions[Convert.ToInt32(e.Viseme)]));
            }
            else
            {
                Console.WriteLine("Error");
                throw new System.InvalidOperationException("mouth is not connected, invalid operation");
            }*/
        }
    }
}
