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
        
        public delegate void ThresholdReachedEventHandler(object sender, MouthPosChangedEventArgs e);
        private int lastViseme = 0;

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
            if (e.Viseme != lastViseme)
            {
                lastViseme = e.Viseme;
                MouthPosChangedEventArgs args = new MouthPosChangedEventArgs();
                args.Pos = (Convert.ToString(positions[Convert.ToInt32(e.Viseme)]));
                OnMouthPosChanged(args);
            }
        }

        protected virtual void OnMouthPosChanged(MouthPosChangedEventArgs e)
        {
            EventHandler<MouthPosChangedEventArgs> handler = MouthPosChanged;
            handler?.Invoke(this, e);
        }

        public event EventHandler<MouthPosChangedEventArgs> MouthPosChanged;
    }

    public class MouthPosChangedEventArgs : EventArgs
    {
        public string Pos { get; set; }
    }
}
