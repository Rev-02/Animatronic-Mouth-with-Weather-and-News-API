using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttsApp
{
    class EyeController
    {
        public int[] Eye1 { get; private set; } = new int[3];
        public int[] Eye2 { get; private set; } = new int[3];

        public EyeController(int[] eye1, int[] eye2)
        {
            Eye1 = eye1;
            Eye2 = eye2;
        }
        
        public EyeController(int[] oneEye)
        {
            Eye1 = oneEye;
            Eye2 = oneEye;
        }
        
        public EyeController()
        {
            Eye1 = new int[] { 0, 0, 0 };
            Eye2 = new int[] { 0, 0, 0 };
        }

    }
}
