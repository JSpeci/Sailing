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

        /*
            Implementace Tie - dělá se podle ranků, 

            R1 R2  R3 R4 součet
        C1                  5
        C2                  6        
        C3                  6
        pokud mají stejně - podívám se na jejich nejlepší závod, a vyberu toho, čí nejlepší závod byl lepší -
        pokud mají oba svůj nejlepší závod stejný, podívám se, který z nich byl dřív v competition

            2 3 4 2
            2 1 2 7


        */
        [Fact]
        public void Should_apply_discards()
        {
            List<Competitor> competitors = new List<Competitor>();
            Competitor c = new Competitor("AAA");
            CompetitorResult cr3 = new CompetitorResult(c, 3);
            CompetitorResult cr2 = new CompetitorResult(c, 2);
            c.RaceResults.Add(new CompetitorResult(c, 1));
            c.RaceResults.Add(new CompetitorResult(c, 2));
            c.RaceResults.Add(cr2);
            c.RaceResults.Add(cr3);
            competitors.Add(c);

            CompetitionRules.ApplyDiscards(competitors, 1);
            Assert.True(cr3.Discarded);

            CompetitionRules.ApplyDiscards(competitors, 2);
            Assert.True(cr3.Discarded);
            Assert.True(cr2.Discarded);
        }

        [Fact]
        public void Should_compute_ranks_123()
        {
            List<Competitor> competitors = new List<Competitor>();
            Competitor c;

            c = new Competitor("AAA");
            c.NetPoints = 10;
            competitors.Add(c);

            c = new Competitor("BBB");
            c.NetPoints = 20;
            competitors.Add(c);

            c = new Competitor("CCC");
            c.NetPoints = 30;
            competitors.Add(c);

            AssertRanks(CompetitionRules.ComputeRanks(competitors, false), 1, 2, 3);
        }

        [Fact]
        public void Should_compute_ranks_122()
        {
            List<Competitor> competitors = new List<Competitor>();
            Competitor c;

            c = new Competitor("AAA");
            c.NetPoints = 10;
            competitors.Add(c);

            c = new Competitor("BBB");
            c.NetPoints = 20;
            competitors.Add(c);

            c = new Competitor("CCC");
            c.NetPoints = 20;
            competitors.Add(c);

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
