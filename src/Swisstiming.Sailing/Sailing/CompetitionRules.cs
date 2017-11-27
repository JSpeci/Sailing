using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sailing
{
    public class CompetitionRules
    {

        public class TieComparer : IComparer<CompetitorsRankInCompetition>
        {
            /*
                  Less than zero 
                    This instance precedes obj in the sort order. 
                  Zero 
                    This instance occurs in the same position in the sort order as obj. 
                  Greater than zero 
                    This instance follows obj in the sort order. 
            */
            public int Compare(CompetitorsRankInCompetition x, CompetitorsRankInCompetition y)
            {
                if (x.competitor.SumOfRanks.CompareTo(y.competitor.SumOfRanks) == 0)
                {
                    return CompareRaceResult(x, y, 0);
                }
                else
                {
                    return x.competitor.SumOfRanks.CompareTo(y.competitor.SumOfRanks);
                }
            }

            private int CompareRaceResult(CompetitorsRankInCompetition x, CompetitorsRankInCompetition y, int index)
            {
                if (index < x.competitor.RaceResults.Count && index < y.competitor.RaceResults.Count)
                {
                    if (x.competitor.RaceResults[index].RaceRank.CompareTo(y.competitor.RaceResults[index].RaceRank) == 0)
                    {
                        return CompareRaceResult(x, y, index + 1);
                    }
                    else
                    {
                        return x.competitor.RaceResults[index].RaceRank.CompareTo(y.competitor.RaceResults[index].RaceRank);
                    }
                }
                else
                {
                    return -1; //terminus
                }
            }

        }

        public static void ApplyRules(Competition competition, IPointSystem pointSystem)
        {
            //deafult pointSystem
            competition.PointSystem = pointSystem;
            ApplyRaceRules(competition.Races, pointSystem);
            ApplyDiscards(competition.Competitors, competition.Discards);
            SumPoints(competition.Competitors);
            competition.Ranks = ComputeRanks(competition.Competitors);
        }

        public static void ApplyRaceRules(List<Race> races, IPointSystem pointSystem)
        {
            foreach (Race r in races)
                RaceRules.ComputePointsAndRanks(r, pointSystem);
        }

        public static void ApplyDiscards(List<Competitor> competitors, int discards)
        {
            if (discards < 0)
            {
                throw new Exception("Discards must be positive number. ");
            }

            // prepared for new discards
            foreach (Competitor c in competitors)
            {
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    cr.Discarded = false;
                }
            }

            foreach (Competitor c in competitors)
            {
                int racesCount = c.RaceResults.Count;

                IEnumerable<CompetitorResult> sortedResults;
                sortedResults = c.RaceResults.OrderBy(r => r.RaceRank);

                //make discards
                foreach (CompetitorResult cr in sortedResults)
                {
                    if (racesCount <= discards)
                    {
                        cr.Discarded = true;
                    }
                    racesCount--;
                }
            }
        }



        public static List<CompetitorsRankInCompetition> ComputeRanks(List<Competitor> competitors)
        {
            IEnumerable<Competitor> sortedCompetitors;

            sortedCompetitors = competitors.OrderBy(r => r.SumOfRanks);

            List<CompetitorsRankInCompetition> Ranks = new List<CompetitorsRankInCompetition>();

            int rank = 1;
            int previous = -1;
            int countOfSame = 0;
            foreach (Competitor competitor in sortedCompetitors)
            {
                if (previous == competitor.SumOfRanks) { countOfSame++; }
                else { countOfSame = 0; }
                Ranks.Add(new CompetitorsRankInCompetition(competitor, rank - countOfSame));
                previous = competitor.SumOfRanks;
                rank++;
            }

            IEnumerable<IGrouping<int, CompetitorsRankInCompetition>> GroupedByRank = Ranks.GroupBy(r => r.rankInCompetition);

            /*
            rank = 1;
            List<CompetitorsRankInCompetition> result = new List<CompetitorsRankInCompetition>();
            foreach (IGrouping<int, CompetitorsRankInCompetition> group in GroupedByRank)
            {
                foreach(CompetitorsRankInCompetition cr in group)
                {

                }
            }*/
            

            return ApplyRankRulesWithTies(Ranks);
        }

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
        public static List<CompetitorsRankInCompetition> ApplyRankRulesWithTies(List<CompetitorsRankInCompetition> Ranks)
        {
            IEnumerable<CompetitorsRankInCompetition> sorted = Ranks.OrderBy(r => r, new TieComparer());
            List<CompetitorsRankInCompetition> result = new List<CompetitorsRankInCompetition>();
            int rank = 1;
            foreach (CompetitorsRankInCompetition cr in sorted)
            {
                result.Add(new CompetitorsRankInCompetition(cr.competitor, rank++));
            }

            return result;
        }



        /*
             Sum of points of every competitor, includes discards
             Goes throug every competitor
             Goes throug every recorded race in competitors list myRaces
             Sum of all points from myRaces
             Discard-omit the n last races
        */
        public static void SumPoints(List<Competitor> competitors)
        {
            // Discards !! n = 1

            foreach (Competitor c in competitors)
            {
                //left out n=1 worst races

                float sum = 0;
                float sumTotal = 0;

                /*c.RaceResults is sorted list. Sort method called when each CompetitorResult object added in Race method loadDataFromCsv*/
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    //the result of competition is simple a sum of race points for each competitor
                    if (!cr.Discarded)
                    {
                        sum += cr.PointsInRace;
                    }
                    sumTotal += cr.PointsInRace;
                }
                c.NetPoints = sum;
                c.TotalPoints = sumTotal;
            }
        }


    }
}
