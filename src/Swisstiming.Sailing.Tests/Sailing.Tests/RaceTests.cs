using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class RaceTests
    {
        /* 
            after each race competitors position is assigned in order they finished (1, 2, 3, ..) it can happen they finished at the same time (1, 1, 2, 3)! 

            the points are assigned in the same order as position based on the pointing system, the simples point system is (1, 2, 3, 4, ...)

         */
        [Fact]
        public void ShouldHavePointsAsFinished1234()
        {
            Race race = GetRace(1, 2, 3, 4);
            AssertPoints(race, 1, 2, 3, 4);
            AssertRank(race, 1, 2, 3, 4);
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
        public void ShouldHaveDividedPointsAsFinished1123()
        {
            Race race = GetRace(1, 1, 2, 3);
            AssertPoints(race,1.5F, 1.5F, 3, 4);
            AssertRank(race, 1, 1, 3, 4);
        }

        [Fact]
        public void ShouldHaveDividedPointsAsFinished1223()
        {
            Race race = GetRace(1, 2, 2, 3);
            AssertPoints(race, 1, 2.5F, 2.5F, 4);
            AssertRank(race, 1, 2, 2, 4);

        }

        [Fact]
        public void ShouldHaveDividedPointsAsFinished1112()
        {
            Race race = GetRace(1, 1, 1, 2);
            AssertPoints(race, 2, 2, 2, 4);
            AssertRank(race, 1, 1, 1, 4);

        }


        private Race GetRace(params int[] positionsFinished)
        {
            int numOfCompetitors = positionsFinished.Length;

            List<Competitor> competitors = new List<Competitor>();
            for (int x = 0; x < numOfCompetitors; x++)
            {
                competitors.Add(new Competitor(x + "" + x + "" + x));
            }

            List<CompetitorResult> raceResult = new List<CompetitorResult>();
            int index = 0;
            foreach (Competitor c in competitors)
            {
                raceResult.Add(new CompetitorResult(c, positionsFinished[index++]));
            }
            Race race = new Race(raceResult);
            
            race.ComputePointsAndRanks(new LowPointSystemDistributor());    //I am doing tests on low point system distribution in this class
            return race;
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
