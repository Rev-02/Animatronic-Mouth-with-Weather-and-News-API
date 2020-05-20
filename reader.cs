using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttsApp
{
    class reader
    {
        public int[] ReadFile()
        {
            int[] positions = new int[22];
            string path = "shapes.txt";
            int i = 0;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {

                    while (sr.EndOfStream != true && i < 22)
                    {
                        string line = sr.ReadLine();
                        positions[i] = (Convert.ToInt32(line));
                        
                        i++;
                    }
                    sr.Close();
                }
            }
            else
            {
                Console.WriteLine("No file found");
            }
            return positions;
        }
        public string[] ReadKeys()
        {
            string[] keys = new string[2];
            string path = "Keys.txt";
            int i = 0;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {

                    while (sr.EndOfStream != true && i < 2)
                    {
                        string line = sr.ReadLine();
                        keys[i] = (Convert.ToString(line));

                        i++;
                    }
                    sr.Close();
                }
            }
            else
            {
                Console.WriteLine("No file found");
            }
            return keys;
        }
    }
}
