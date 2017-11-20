using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CompetitionTests
    {

        [Fact]
        public void SumTotalPointsTest()
        {
            Competition competition = GetCompetition();
            //same as ApplyDiscards(0);
            competition.ApplyRules(new LowPointSystem());
            AssertTotalPoints(competition, 4.5F, 6F, 7.5F, 12F);
            AssertRanks(competition, 1, 2, 3, 4);

            competition.ApplyRules(new CustomPointSystem());
            AssertTotalPoints(competition, 8.5F+22/3F+10F, 8.5F+22/3F+6F, 5F+22/3F+6F, 12F);
            AssertRanks(competition, 1, 2, 3, 4);

        }
        [Fact]
        public void SumNetPointsDiscard1()
        {
            Competition competition = GetCompetition();
            competition.ApplyRules(new LowPointSystem());
            competition.ApplyDiscards(1);
            AssertNetPoints(competition, 2.5F, 3.5F, 4.5F, 8F);
            AssertRanks(competition, 1, 2, 3, 4);

            competition.ApplyRules(new CustomPointSystem());
            AssertNetPoints(competition, 8.5F + 10F, 8.5F + 22 / 3F, 22 / 3F + 6F, 4F + 4F);
            AssertRanks(competition, 1, 2, 3, 4);
        }
        [Fact]
        public void SumNetPointsDiscard2() //Summary with 2 discarded races
        {
            Competition competition = GetCompetition();
            competition.ApplyRules(new LowPointSystem());
            competition.ApplyDiscards(2);
            AssertNetPoints(competition, 1F, 1.5F, 2F, 4F);
            AssertRanks(competition, 1, 2, 3, 4);

            competition.ApplyRules(new CustomPointSystem());
            AssertNetPoints(competition, 10F, 8.5F, 22 / 3F, 4F);
            AssertRanks(competition, 1, 2, 3, 4);


        }

        private void AssertRanks(Competition competition, params int[] expectedRanks)
        {
            int index = 0;
            foreach (Competition.CompetitorsRankInCompetition cr in competition.Ranks)
            {
                Assert.Equal(cr.rankInCompetition, expectedRanks[index++]);
            }
        }

        private void AssertTotalPoints(Competition competition, params float[] expectedPoints)
        {
            int index = 0;
            foreach (Competitor competitor in competition.Competitors)
            {
                Assert.Equal(competitor.TotalPoints, expectedPoints[index++]);
            }
        }

        private void AssertNetPoints(Competition competition, params float[] expectedPoints)
        {
            int index = 0;
            foreach (Competitor competitor in competition.Competitors)
            {
                Assert.Equal(competitor.NetPoints, expectedPoints[index++]);
            }
        }

        private Competition GetCompetition()
        {

            List<Competitor> competitors = GetCompetitors(4);  // 4 testing competitors
            List<Race> races = new List<Race>();
            Competition competition = new Competition(competitors, races);
            competition.Races.Add(GetRace(competition.Competitors, 1, 1, 2, 3));
            competition.Races.Add(GetRace(competition.Competitors, 1, 1, 1, 2));
            competition.Races.Add(GetRace(competition.Competitors, 1, 2, 2, 3));
            return competition;
        }

        private Race GetRace(List<Competitor> competitors, params int[] positionsFinished)
        {
            List<CompetitorResult> raceResult = new List<CompetitorResult>();

            int index = 0;
            foreach (Competitor competitor in competitors)
            {
                CompetitorResult cr = new CompetitorResult(competitor, positionsFinished[index++]);
                raceResult.Add(cr);
                competitor.RaceResults.Add(cr);
                competitor.RaceResults.Sort();
            }

            Race race = new Race(raceResult);
            return race;
        }
        
        /*
         Names of competitors 111,222,333  XXX
         */
        private List<Competitor> GetCompetitors(int numOfCompetitors)
        {
            List<Competitor> competitors = new List<Competitor>();
            for (int x = 0; x < numOfCompetitors; x++)
            {
                competitors.Add(new Competitor(x + "" + x + "" + x));
            }
            return competitors;
        }

    }
}
