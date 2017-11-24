using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CompetitionRulesTests
    {

        [Fact]
        public void Should_apply_discards()
        {
            List<Competitor> competitors = GetCompetitors(4);

            CompetitionRules.ApplyDiscards(competitors, 1);
            CheckDiscards(competitors, 1);

            CompetitionRules.ApplyDiscards(competitors, 2);
            CheckDiscards(competitors, 2);
        }

        [Fact]
        public void Should_compute_ranks_123()
        {
            List<Competitor> competitors = GetCompetitors(3);
            SetUpNetPoints(competitors, 10, 20, 30);
            AssertRanks(CompetitionRules.ComputeRanks(competitors, false), 1, 2, 3);
        }

        [Fact]
        public void Should_compute_ranks_122()
        {
            List<Competitor> competitors = GetCompetitors(3);
            SetUpNetPoints(competitors, 10, 20, 20);
            AssertRanks(CompetitionRules.ComputeRanks(competitors, false), 1, 2, 2);
        }

        [Fact]
        public void Should_sum_points()     //sum of total points and sum of net points when 1 discarded
        {
            List<Competitor> competitors = GetCompetitors(4);
            CompetitionRules.SumPoints(competitors);
            //total points 
            //11+22+33+44 = 110
            //22+44+66+88 = 220
            //33+66+99+132 = 330
            //44+88+132+176 = 440
            AssertTotalPoints(competitors, 110, 220, 330, 440); 
            AssertNetPoints(competitors, 110, 220, 330, 440); // 0 discarded

            CompetitionRules.ApplyDiscards(competitors, 1);// 1 discarded
            CompetitionRules.SumPoints(competitors);
            //net points 
            //11+22+33 = 66
            //22+44+66 = 132
            //33+66+99 = 198
            //44+88+132 = 264
            AssertNetPoints(competitors, 66, 132, 198, 264); // 1 discarded


        }

        private void CheckDiscards(List<Competitor> competitors, int countOfdiscards)
        {
            //check if the wors pointed race of each competitor is discarded
            List<CompetitorResult> shouldBeDiscarded = new List<CompetitorResult>();
            foreach (Competitor c in competitors)
            {
                //find max pointed raceResult in c.RaceResults

                IEnumerable<CompetitorResult> sorted = c.RaceResults.OrderByDescending(i => i.PointsInRace);

                for(int x = 0; x < countOfdiscards; x++)
                {
                    shouldBeDiscarded.Add(sorted.ElementAt<CompetitorResult>(x));
                }
            }
            foreach(CompetitorResult cr in shouldBeDiscarded)
            {
                Assert.True(cr.Discarded);
            }
        }

        private void SetUpNetPoints(List<Competitor> competitors, params int[] netPoints)
        {
            if(competitors.Count == netPoints.Length)
            {
                int index = 0;
                foreach(Competitor c in competitors)
                {
                    c.NetPoints = netPoints[index];
                    index++;
                }
            }
        }

        private List<Competitor> GetCompetitors(int numOfCompetitors)
        {
            List<Competitor> competitors = new List<Competitor>();
            for(int x=1; x <= numOfCompetitors; x++)
            {
                competitors.Add(new Competitor(x.ToString()));
            }
            getRaceResults(competitors, 4);
            return competitors;
        }

        private void getRaceResults(List<Competitor> competitors, int numOfRaceResults)     //inserting raceResults into competitors
        {
            CompetitorResult cr;
            int index = 1;
            foreach(Competitor c in competitors)
            {
                for(int x = 1; x <= numOfRaceResults; x++)
                {
                    cr = new CompetitorResult(c, x);
                    cr.PointsInRace = index * x * 11; 
                    cr.RaceRank = x;
                    c.RaceResults.Add(cr);
                }
                index++;
                //11+22+33+44 = 110
                //22+44+66+88 = 220
                //33+66+99+132 = 330
                //44+88+132+176 = 440
            }
        }

        //assert methods

        private void AssertRanks(List<CompetitorsRankInCompetition> ranks, params int[] expectedRanks)
        {
            int index = 0;
            foreach (CompetitorsRankInCompetition cr in ranks)
            {
                Assert.Equal(cr.rankInCompetition, expectedRanks[index++]);
            }
        }

        private void AssertTotalPoints(List<Competitor> competitors, params float[] expectedPoints)
        {
            int index = 0;
            foreach (Competitor competitor in competitors)
            {
                Assert.Equal(competitor.TotalPoints, expectedPoints[index++]);
            }
        }

        private void AssertNetPoints(List<Competitor> competitors, params float[] expectedPoints)
        {
            int index = 0;
            foreach (Competitor competitor in competitors)
            {
                Assert.Equal(competitor.NetPoints, expectedPoints[index++]);
            }
        }


    }
}
