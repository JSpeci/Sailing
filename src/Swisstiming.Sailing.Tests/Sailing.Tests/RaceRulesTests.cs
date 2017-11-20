using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class RaceRulesTests
    {
        /* 
            after each race competitors position is assigned in order they finished (1, 2, 3, ..) it can happen they finished at the same time (1, 1, 2, 3)! 

            the points are assigned in the same order as position based on the pointing system, the simples point system is (1, 2, 3, 4, ...)

             if some of the competitors finished on the same position the points are splitted between them
             (position: 1, 1, 2; points: 1.5, 1.5, 3)

               the rank is assigned based on points

             the rank is assigned based on points if some of the competitors has the same number of points,
             they have the same rank but the next rank is left out (points: 1.5, 1.5, 3, 4, rank: 1, 1, 3, 4)
        */

        [Fact]
        public void Should_Have_Points_As_Finished_112()
        {
            var pointSystem = new Mock<IPointSystem>();
            pointSystem.Setup(i => i.GetPointsFromPosition(1)).Returns(10);
            pointSystem.Setup(i => i.GetPointsFromPosition(2)).Returns(7);
            pointSystem.Setup(i => i.GetPointsFromPosition(3)).Returns(5);
            Race race = new Race(GetRaceResult(1, 1, 2));
            RaceRules.ComputePointsAndRanks(race, pointSystem.Object);
            AssertPoints(race, 8.5F, 8.5F, 5);
            AssertRank(race, 1, 1, 3);

            pointSystem.VerifyAll();
        }

       

        [Fact]
        public void Should_Have_Points_As_Finished_1112()
        {
            var pointSystem = new Mock<IPointSystem>();
            pointSystem.Setup(i => i.GetPointsFromPosition(1)).Returns(50);
            pointSystem.Setup(i => i.GetPointsFromPosition(2)).Returns(40);
            pointSystem.Setup(i => i.GetPointsFromPosition(3)).Returns(30);
            pointSystem.Setup(i => i.GetPointsFromPosition(4)).Returns(20);

            Race race = new Race(GetRaceResult(1, 1, 1, 2));
            RaceRules.ComputePointsAndRanks(race, pointSystem.Object);
            AssertPoints(race, 40 , 40 , 40 , 20);
            AssertRank(race, 1, 1, 1, 4);

            pointSystem.VerifyAll();
        }

        /*
        
            
        [Fact]
        public void ShouldHavePointsAsFinished1234()
        {
            List<CompetitorResult> raceResult = GetRaceResult(1, 2, 3, 4);
            PointSystemDistributor.ComputePointsAndRanks(raceResult, new CustomPointSystem());
            AssertPoints(raceResult, 10, 7, 5, 4);
            AssertRank(raceResult, 1, 2, 3, 4);

            PointSystemDistributor.ComputePointsAndRanks(raceResult, new LowPointSystem());
            AssertPoints(raceResult, 1, 2, 3, 4);
            AssertRank(raceResult, 1, 2, 3, 4);
        }

             [Fact]
        public void ShouldHavePointsAsFinished12234()
        {
            List<CompetitorResult> raceResult = GetRaceResult(1, 2, 2, 3, 4);
            PointSystemDistributor.ComputePointsAndRanks(raceResult, new CustomPointSystem());
            AssertPoints(raceResult, 10, 6, 6, 4, 3);
            AssertRank(raceResult, 1, 2, 2, 4, 5);

            PointSystemDistributor.ComputePointsAndRanks(raceResult, new LowPointSystem());
            AssertPoints(raceResult, 1, 2.5F, 2.5F, 4, 5);
            AssertRank(raceResult, 1, 2, 2, 4, 5);
        }
            
        */

        private List<CompetitorResult> GetRaceResult(params int[] positionFinished)
        {
            List<CompetitorResult> raceResult = new List<CompetitorResult>();

            foreach (int i in positionFinished)
            {
                raceResult.Add(new CompetitorResult(new Competitor("name"), i));
            }

            return raceResult;
        }

        private void AssertPoints(Race race, params float[] expectedPoints)
        {
            int index = 0;
            foreach (CompetitorResult cr in race.RaceResult)
            {
                Assert.Equal(cr.PointsInRace, expectedPoints[index++]);
            }
        }

        private void AssertRank(Race race, params int[] expectedRank)
        {
            int index = 0;
            foreach (CompetitorResult cr in race.RaceResult)
            {
                Assert.Equal(cr.RaceRank, expectedRank[index++]);
            }
        }
    }
}
