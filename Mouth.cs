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
        private int[] positions = new int[21];
        SpeechSynthesizer speak = new SpeechSynthesizer();
        public string voice { get; set;  }
        public string PortQueue { get; private set; }

        public  Mouth(string Voice)
        {
            voice = Voice;
            speak.SetOutputToDefaultAudioDevice();
            speak.VisemeReached += new EventHandler<VisemeReachedEventArgs>(synthVisemeReached);
            reader rdr = new reader();
            positions = rdr.ReadFile();
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
            PortQueue = Convert.ToString(positions[Convert.ToInt32(e.Viseme)]);
        }
    }
}
