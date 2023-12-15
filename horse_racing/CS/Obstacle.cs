using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace horse_racing.cs
{
    public class Obstacle
    {
        public int Position { get; set; }

        public bool Obstaclee(int jump)
        {
            Random random = new Random();
            bool jumping = false; 
            int[] arr = new int[10];
            arr[random.Next(10)] = 1;

            for(int i=0; i<jump; i++)
            {
                int randomNum = random.Next(10);
                if (arr[randomNum] == 1)
                {
                    jumping = true;
                    break;
                }
            }

            return jumping;
        }
    }
}
