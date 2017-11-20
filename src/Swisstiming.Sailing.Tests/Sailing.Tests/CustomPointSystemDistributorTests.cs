using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CustomPointSystemDistributorTests
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
        public void ShouldHavePointsAsFinished1234()
        {
            IScoreDistributor dis = new CustomPointSystemDistributor();
            List<CompetitorResult> raceResult = GetRaceResult(1, 2, 3, 4);
            dis.ComputePointsAndRanks(raceResult);
            AssertPoints(raceResult, 1, 2, 3, 4);
            AssertRank(raceResult, 1, 2, 3, 4);
        }

        private List<CompetitorResult> GetRaceResult(params int[] positionFinished)
        {
            List<CompetitorResult> raceResult = new List<CompetitorResult>();

            foreach (int i in positionFinished)
            {
                raceResult.Add(new CompetitorResult(new Competitor("name"), i));
            }

            return raceResult;
        }

        private void AssertPoints(List<CompetitorResult> raceResult, params float[] expectedPoints)
        {
            int index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                Assert.Equal(cr.PointsInRace, expectedPoints[index++]);
            }
        }

        private void AssertRank(List<CompetitorResult> raceResult, params int[] expectedRank)
        {
            int index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                Assert.Equal(cr.RaceRank, expectedRank[index++]);
            }
        }

    }
}
