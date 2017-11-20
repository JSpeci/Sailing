using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    /*
      Custom point system
      positionFinished: 1,2,3,4,5,6,7,8,9...
      points:          10,7,5,4,3,2,1,0,0...
    */
    public class CustomPointSystem : IPointSystem
    {
        private readonly int[] scoringScale = { 10, 7, 5, 4, 3, 2, 1 };

        public bool DescendingPoints()
        {
            return true;
        }

        /* 10,7,5,4,3,2,1,0,0,0,0,0,0,0 */
        public int GetPointsFromPosition(int positionFinished)
        {
            if (positionFinished >= 1 && positionFinished <= scoringScale.Length)  // 7 positions in scoring scale
            {
                return scoringScale[positionFinished - 1];
            }
            else
            {
                return 0;
            }
        }
    }
}
