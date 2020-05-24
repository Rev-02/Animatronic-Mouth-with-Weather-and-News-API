using System;
using System.Collections.Generic;
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
            Eyes[0] = new int[] { 0, 0, 0 };
            Eyes[1] = new int[] { 0, 0, 0 };
        }

        public void setOne(int index,int[] colour)
        {
            for (int i = 0; i<3;i++)
            {
                Eyes[index][i] = colour[i];
            }

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
            int[][] previousCol = new int[2][];
            previousCol = Eyes;
            int[] black = { 0, 0, 0 };
            setBoth(black);
            Thread.Sleep(750);
            for(int i =0; i<previousCol.Length;i++)
            {
                setOne(i, previousCol[i]);
            }

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
                Thread.Sleep(200);
            }
        }
    }
}
