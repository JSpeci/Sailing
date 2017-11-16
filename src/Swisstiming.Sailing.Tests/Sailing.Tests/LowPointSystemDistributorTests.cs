using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class LowPointSystemDistributorTests
    {
        /* 
        after each race competitors position is assigned in order they finished (1, 2, 3, ..) it can happen they finished at the same time (1, 1, 2, 3)! 

        the points are assigned in the same order as position based on the pointing system, the simples point system is (1, 2, 3, 4, ...)

        */
        [Fact]
        public void ShouldHavePointsAsFinished1234()
        {
            LowPointSystemDistributor dis = new LowPointSystemDistributor();
            List<CompetitorResult> raceResult = GetRaceResult(1, 2, 3, 4);
            dis.ComputePointsAndRanks(raceResult);
            AssertPoints(raceResult, 1, 2, 3, 4);
            AssertRank(raceResult, 1, 2, 3, 4);
        }


        /*
         if some of the competitors finished on the same position the points are splitted between them
         (position: 1, 1, 2; points: 1.5, 1.5, 3)
        */

        /*
           the rank is assigned based on points
        */

        /*
         the rank is assigned based on points if some of the competitors has the same number of points,
         they have the same rank but the next rank is left out (points: 1.5, 1.5, 3, 4, rank: 1, 1, 3, 4)
         */
        [Fact]
        public void ShouldHaveDistributePointsAsFinished1123()
        {
            LowPointSystemDistributor dis = new LowPointSystemDistributor();
            List<CompetitorResult> raceResult = GetRaceResult(1, 1, 2, 3);
            dis.ComputePointsAndRanks(raceResult);
            AssertPoints(raceResult, 1.5F, 1.5F, 3, 4);
            AssertRank(raceResult, 1, 1, 3, 4);
        }

        [Fact]
        public void ShouldHaveDividedPointsAsFinished1223()
        {
            LowPointSystemDistributor dis = new LowPointSystemDistributor();
            List<CompetitorResult> raceResult = GetRaceResult(1, 2, 2, 3);
            dis.ComputePointsAndRanks(raceResult);
            AssertPoints(raceResult, 1, 2.5F, 2.5F, 4);
            AssertRank(raceResult, 1, 2, 2, 4);
        }

        [Fact]
        public void ShouldHaveDividedPointsAsFinished1112()
        {
            LowPointSystemDistributor dis = new LowPointSystemDistributor();
            List<CompetitorResult> raceResult = GetRaceResult(1, 1, 1, 2);
            dis.ComputePointsAndRanks(raceResult);
            AssertPoints(raceResult, 2, 2, 2, 4);
            AssertRank(raceResult, 1, 1, 1, 4);
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
