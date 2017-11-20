using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    /*
      Simple low point system
      positionFinished: 1,2,3,4,5....
      points:           1,2,3,4,5....
    */
    public class LowPointSystem : IPointSystem
    {
        public int GetPointsFromPosition(int positionFinished)
        {
            if (positionFinished > 0)
            {
                return positionFinished;
            }
            else return 0;
        }
    }
}
