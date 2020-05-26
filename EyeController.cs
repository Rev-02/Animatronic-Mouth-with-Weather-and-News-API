using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ttsApp
{
    class EyeController
    {
        public int[][] Eyes { get; private set; } = new int[2][];
        Random random = new Random();
        public delegate void ThresholdReachedEventHandler(object sender, EyesChangedEventArgs e);

        public EyeController(int[] eye1, int[] eye2)
        {
            Eyes[0] = eye1;
            Eyes[1] = eye2;
        }

        public EyeController(int[] oneEye)
        {
            Eyes[0] = oneEye;
            Eyes[1] = oneEye;
        }

        public EyeController()
        {
            Eyes[0] = new int[] { 0,120,50 };
            Eyes[1] = new int[] { 0,120,50 };
        }

        public void setOne(int index,int[] colour)
        {
            //int[] previous = Eyes[index];
            //TODO: add checking to see if previous val == current val
            for (int i = 0; i<3;i++)
            {
                Eyes[index][i] = colour[i];
            }
            EyesChangedEventArgs args = new EyesChangedEventArgs();
            args.Eyes = Eyes;
            OnEyesChanged(args);
            
        }

        public void setBoth(int[] colour)
        {
            for (int i = 0; i < 2; i++)
            {
                setOne(i,colour);
            }
        }

        public void blink()
        {
            int[][] previousCol  = new int[][] { new int[]{ 0, 0, 0 }, new int[]{ 0, 0, 0 } };
            for (int i = 0; i < 2; i++)
            {
                for (int b = 0; b < Eyes[i].Length; b++)
                {
                    previousCol[i][b] = Eyes[i][b];
                }
            }
            int[] black = { 10, 10, 10 };
            setBoth(black);
            Thread.Sleep(400);
            setOne(0,previousCol[0]);
            setOne(1,previousCol[1]);

        }

        public void disco()
        {
            int[] eye1 = new int[3];
            for (int i = 0; i < 30; i++)
            {
                for (int a = 0; a < 2; a++)
                {
                    for (int b = 0; b < 3; b++)
                    {
                        eye1[b] = random.Next(127);
                    }
                    setOne(a, eye1);

                }
                Thread.Sleep(350);
            }
        }

        protected virtual void OnEyesChanged(EyesChangedEventArgs e)
        {
            EventHandler<EyesChangedEventArgs> handler = EyesChanged;
            handler?.Invoke(this, e);
        }

        public event EventHandler<EyesChangedEventArgs> EyesChanged;
    }
    public class EyesChangedEventArgs : EventArgs
    {
        public int[][] Eyes { get; set; }
    }
}
