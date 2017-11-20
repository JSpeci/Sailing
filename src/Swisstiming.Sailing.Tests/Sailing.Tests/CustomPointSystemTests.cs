using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CustomPointSystemTests
    {
        /*
            Vyrobit nastavitelny system bodovani raců tak, ze nyní je to prostý linearni low-point system
            finishing time - 1 2 3 3 4 5 
            points ted  1 2 3 4 5 6 

            nove points 10,7,5,4,3,2,1,0,0,0,0,0,0  - custom řada ze zadani
            otočit sortovani - na prvnim miste v poli ranks je ten co ma nejvice bodu - pravidla pro stejny pocet bodu zustavaji

            Udelat to rozsiritelne  - nastaveni bude nejaky objekt s pravidly - bude mozne pouzit obe pravidla, ale vzdy na celou Competition

        */

        [Fact]
        public void CustomSeriesTest()
        {
            CustomPointSystem cp = new CustomPointSystem();
            Assert.Equal(cp.GetPointsFromPosition(1), 10);
            Assert.Equal(cp.GetPointsFromPosition(2), 7);
            Assert.Equal(cp.GetPointsFromPosition(3), 5);
            Assert.Equal(cp.GetPointsFromPosition(4), 4);
            Assert.Equal(cp.GetPointsFromPosition(5), 3);
            Assert.Equal(cp.GetPointsFromPosition(6), 2);
            Assert.Equal(cp.GetPointsFromPosition(7), 1);

            Assert.Equal(cp.GetPointsFromPosition(10), 0);

            Assert.Equal(cp.GetPointsFromPosition(100), 0);
        }
    }
}
