using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Speech.Synthesis.TtsEngine;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ttsApp
{
    class FaceController
    {
        private static SerialPort port;
        private bool portSuccesful = false;
        private string lastWrite = "";
        public FaceController(string portName, int baudRate)
        {
            port = new SerialPort(portName, baudRate);
            try { 
                port.Open();
                portSuccesful = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public void writeFace(string mouthPos, int[][] colorArray)
        {
            string writeData = "";
            writeData += mouthPos+",";
            for (int i=0; i < colorArray.Length; i++)
            {
                writeData += String.Join(",", colorArray[i]);
                if(i!=colorArray.Length - 1)
                {
                    writeData += ",";
                }
            }
            if (writeData != lastWrite)
            {
                lastWrite = writeData;
                port.WriteLine(writeData);
            }
        }

        public void POST()
        {
            if (portSuccesful)
            {
                try
                {
                    port.WriteLine("120,0,0,0;0,0,0");
                    Thread.Sleep(1500);
                    port.WriteLine("0,0,0,0;0,0,0");
                    string writeString;
                    int[] colArr = { 0, 0, 0 ,0,0,0};
                    for (int i = 0; i < colArr.Length; i++)
                    {
                        for (int b = 0; b < 128; b++)
                        {

                            colArr[i] = b;
                            //writeString = "0" + "," + colArr[0] + ":" + colArr[1] + ":" + colArr[2] + ";" + colArr[0] + ":" + colArr[1] + ":" + colArr[2];
                            writeString = string.Format("0,{0},{1},{2},{3},{4},{5}", colArr[0], colArr[1], colArr[2], colArr[3], colArr[4], colArr[5]);
                            port.WriteLine(writeString);
                            Thread.Sleep(010);
                        }
                        colArr[i] = 0;
                    }
                    port.WriteLine("0,0,0,0,0,0,0");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                //TODO: remove debug console prints 
                Console.WriteLine("120,0,0,0,0,0,0");
                Thread.Sleep(1500);
                Console.WriteLine("0,0,0,0,0,0,0");
                string writeString;
                int[] colArr = { 0, 0, 0 };
                for (int i = 0; i < 3; i++)
                {
                    for (int b = 0; b < 128; b++)
                    {

                        colArr[i] = b;
                        //writeString = "0" + "," + colArr[0] + ":" + colArr[1] + ":" + colArr[2] + ";" + colArr[0] + ":" + colArr[1] + ":" + colArr[2];
                        writeString = string.Format("0,{0},{1},{2},{0},{1},{2}", colArr[0], colArr[1], colArr[2]);
                        Console.WriteLine(writeString);
                    }
                    colArr[i] = 0;
                }
            }
        }
    }
}
