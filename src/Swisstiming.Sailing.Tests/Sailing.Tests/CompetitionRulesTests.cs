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
            SetUpRaceRanks(competitors[0].RaceResults, 1, 1, 1, 1); // 4
            SetUpRaceRanks(competitors[1].RaceResults, 2, 1, 1, 1); // 5
            SetUpRaceRanks(competitors[2].RaceResults, 1, 2, 2, 1); // 6

            AssertRanks(CompetitionRules.ComputeRanks(competitors), 1, 2, 3);
        }

        [Fact]
        public void Should_compute_ranks_by_122_to_123()
        {
            List<Competitor> competitors = GetCompetitors(3);
            SetUpRaceRanks(competitors[0].RaceResults, 2, 1, 1, 1); // 5
            SetUpRaceRanks(competitors[1].RaceResults, 1, 1, 2, 2); // 6
            SetUpRaceRanks(competitors[2].RaceResults, 1, 2, 2, 1); // 6

            AssertRanks(CompetitionRules.ComputeRanks(competitors), 1, 2, 3); // not 1 2 2
        }

        [Fact]
        public void Should_compute_ranks_by_122_to_123_B()
        {
            List<Competitor> competitors = GetCompetitors(3);
            SetUpRaceRanks(competitors[0].RaceResults, 20, 10, 10, 10); // 50
            SetUpRaceRanks(competitors[1].RaceResults, 10, 10, 20, 20); // 60
            SetUpRaceRanks(competitors[2].RaceResults, 10, 20, 20, 10); // 60

            AssertRanks(CompetitionRules.ComputeRanks(competitors), 1, 2, 3); // not 1 2 2
        }

        [Fact]
        public void Should_decide_ties()
        {
            List<Competitor> competitors = GetCompetitors(6);
            SetUpRaceRanks(competitors[0].RaceResults, 1, 1, 1, 3);
            SetUpRaceRanks(competitors[1].RaceResults, 1, 1, 2, 2);
            SetUpRaceRanks(competitors[2].RaceResults, 1, 2, 2, 1);
            SetUpRaceRanks(competitors[3].RaceResults, 2, 1, 1, 2);
            SetUpRaceRanks(competitors[4].RaceResults, 2, 1, 2, 1);
            SetUpRaceRanks(competitors[5].RaceResults, 2, 2, 1, 1);
            
            List<CompetitorsRankInCompetition> ties = new List<CompetitorsRankInCompetition>();
            foreach (Competitor c in competitors)
                ties.Add(new CompetitorsRankInCompetition(c, 1));

            //tested method
            AssertRanks(CompetitionRules.ApplyRankRulesWithTies(ties), 1, 2, 3, 4, 5, 6); //not 1 1 1 1 1 1 1
        }

        [Fact]
        public void Should_decide_ties_when_all_results_are_same_And_some_discarded()
        {
            List<Competitor> competitors = GetCompetitors(2);
            SetUpRaceRanks(competitors[0].RaceResults, 1, 1, 1, 3);
            SetUpRaceRanks(competitors[1].RaceResults, 1, 1, 1, 3);
            competitors[0].RaceResults[0].Discarded = true;

            List<CompetitorsRankInCompetition> ties = new List<CompetitorsRankInCompetition>();
            foreach (Competitor c in competitors)
                ties.Add(new CompetitorsRankInCompetition(c, 1));

            AssertRanks(CompetitionRules.ApplyRankRulesWithTies(ties), 2,1); //not 1 1
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

                for (int x = 0; x < countOfdiscards; x++)
                {
                    shouldBeDiscarded.Add(sorted.ElementAt<CompetitorResult>(x));
                }
            }
            foreach (CompetitorResult cr in shouldBeDiscarded)
            {
                Assert.True(cr.Discarded);
            }
        }

        //assign raceRanks into Competitors raceResults
        private void SetUpRaceRanks(List<CompetitorResult> raceResultsOfCompetitor, params int[] raceRanks)
        {
            if (raceResultsOfCompetitor.Count == raceRanks.Length)
            {
                int index = 0;
                foreach (CompetitorResult cr in raceResultsOfCompetitor)
                {
                    cr.RaceRank = raceRanks[index++];
                }
            }
        }

        //competitor generator
        private List<Competitor> GetCompetitors(int numOfCompetitors)
        {
            //setting up competitors list
            List<Competitor> competitors = new List<Competitor>();
            for (int x = 1; x <= numOfCompetitors; x++)
            {
                competitors.Add(new Competitor(x.ToString()));
            }
            //setting up net and total points by adding race results to competitors
            getRaceResults(competitors, 4);
            return competitors;
        }

        //inserting raceResults into competitors
        private void getRaceResults(List<Competitor> competitors, int numOfRaceResults)
        {
            CompetitorResult cr;
            int index = 1;
            foreach (Competitor c in competitors)
            {
                for (int x = 1; x <= numOfRaceResults; x++)
                {
                    cr = new CompetitorResult(c, x);
                    cr.PointsInRace = index * x * 11;
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
            IEnumerable<CompetitorsRankInCompetition> sortedByName = ranks.OrderBy(r => r.competitor.Name);

            List<CompetitorsRankInCompetition> sortedInList = new List<CompetitorsRankInCompetition>();
            foreach (CompetitorsRankInCompetition cr in sortedByName)
                sortedInList.Add(cr);

            int index = 0;
            foreach (CompetitorsRankInCompetition cr in sortedInList)
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
