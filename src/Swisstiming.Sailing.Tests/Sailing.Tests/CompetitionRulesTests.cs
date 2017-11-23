using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CompetitionRulesTests
    {


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
        public void Should_compute_ranks_1234()
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
        public void Should_compute_ranks_1224()
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
        public void Should_sum_points()
        {
            List<Competitor> competitors = new List<Competitor>();
            Competitor c;
            CompetitorResult cr;

            for(int x = 0; x < 4; x++)
            {
                c = new Competitor(x.ToString());
                cr = new CompetitorResult(c, 1);
                cr.PointsInRace = 11;
                c.RaceResults.Add(cr);
                competitors.Add(c);
            }

            CompetitionRules.SumPoints(competitors);

            AssertTotalPoints(competitors, 11, 11, 11, 11);
            AssertNetPoints(competitors, 11,11,11,11);
        }

        /*
        [Fact]
        public void Should_have_applied_race_rules()
        {
            List<Competitor> competitors = GetCompetitors(4);
            List<Race> races = new List<Race>();
            races.Add(GetRace(competitors, 1, 1, 2, 3));
            races.Add(GetRace(competitors, 1, 1, 1, 2));
            races.Add(GetRace(competitors, 1, 2, 2, 3));

            var pointSystem = new Mock<IPointSystem>();
            pointSystem.Setup(i => i.GetPointsFromPosition(1)).Returns(10);
            pointSystem.Setup(i => i.GetPointsFromPosition(2)).Returns(7);
            pointSystem.Setup(i => i.GetPointsFromPosition(3)).Returns(5);
            pointSystem.Setup(i => i.GetPointsFromPosition(4)).Returns(4);

            CompetitionRules.ApplyRaceRules(races, pointSystem.Object);
        }
      
                [Fact]
                public void SumTotalPointsTest()
                {
                    Competition competition = GetCompetition();
                    CompetitionRules.ApplyRules(competition, new CustomPointSystem());
                    AssertTotalPoints(competition, 8.5F+22/3F+10F, 8.5F+22/3F+6F, 5F+22/3F+6F, 12F);
                    AssertRanks(competition, 1, 2, 3, 4);

                }
                [Fact]
                public void SumNetPointsDiscard1()
                {
                    Competition competition = GetCompetition();
                    competition.Discards = 1;
                    CompetitionRules.ApplyRules(competition, new LowPointSystem());
                    AssertNetPoints(competition, 2.5F, 3.5F, 4.5F, 8F);
                    AssertRanks(competition, 1, 2, 3, 4);

                    CompetitionRules.ApplyRules(competition,new CustomPointSystem());
                    AssertNetPoints(competition, 8.5F + 10F, 8.5F + 22 / 3F, 22 / 3F + 6F, 4F + 4F);
                    AssertRanks(competition, 1, 2, 3, 4);
                }
                [Fact]
                public void SumNetPointsDiscard2() //Summary with 2 discarded races
                {
                    Competition competition = GetCompetition();
                    competition.Discards = 2;
                    CompetitionRules.ApplyRules(competition, new LowPointSystem());
                    AssertNetPoints(competition, 1F, 1.5F, 2F, 4F);
                    AssertRanks(competition, 1, 2, 3, 4);

                    CompetitionRules.ApplyRules(competition, new CustomPointSystem());
                    AssertNetPoints(competition, 10F, 8.5F, 22 / 3F, 4F);
                    AssertRanks(competition, 1, 2, 3, 4);

                }
                */
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
