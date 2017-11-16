using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class LowPointSystemDistributorTests
    {
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

            foreach(int i in positionFinished)
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
