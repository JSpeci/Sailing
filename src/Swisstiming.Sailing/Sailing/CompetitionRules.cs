using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sailing
{
    public class CompetitionRules
    {
        private struct CompetitorIntegerForTie
        {
            public int raceResultsInteger;
            public Competitor competitor;
            public CompetitorIntegerForTie(Competitor c, int i)
            {
                this.competitor = c;
                this.raceResultsInteger = i;
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

            //netPoints should not be used
            //sortedCompetitors = competitors.OrderByDescending(p);

            sortedCompetitors = competitors.OrderBy(r => r.SumOfRanks);


            List<CompetitorsRankInCompetition> Ranks = new List<CompetitorsRankInCompetition>();

            // Temporary arrays for computing ranks from points
            int[] rankArray = new int[competitors.Count];
            float[] sumOfRanksArray = new float[competitors.Count];
            int index = 0;
            foreach (Competitor c in sortedCompetitors)
                sumOfRanksArray[index++] = c.SumOfRanks;

            int iter = 1;   //rank for competitors with different sum of points
            float previous = -1F;   //temporary variable
            for (int x = 0; x < sumOfRanksArray.Length; x++)
            {
                //if competitor has same sum of points as previous competitor, they have same rank
                if (previous == sumOfRanksArray[x])
                {
                    rankArray[x] = rankArray[x - 1];
                }
                else { rankArray[x] = iter; /* rank assigned by order */ }
                iter++;
                previous = sumOfRanksArray[x];
            }

            //Refilling data structures from temporary arrays
            index = 0;
            foreach (Competitor c in sortedCompetitors)
                Ranks.Add(new CompetitorsRankInCompetition(c, rankArray[index++]));

            //TIE decision making
            //loop through sortedCompetitors and make list of Competitors with same rank, 
            // then make decision, and distribute final ranks

            List<CompetitorsRankInCompetition> forTie = new List<CompetitorsRankInCompetition>();
            CompetitorsRankInCompetition previousCompetitor = new CompetitorsRankInCompetition();
            foreach (CompetitorsRankInCompetition cr in Ranks)
            {
                if(cr.rankInCompetition == previousCompetitor.rankInCompetition)
                {
                    if (forTie.Count == 0)
                    {
                        forTie.Add(previousCompetitor);
                    }
                    forTie.Add(cr);
                }
                else
                {
                    ApplyTies(forTie);
                }
                previousCompetitor = cr;
            }

            //in case of all results are in ties
            ApplyTies(forTie);

            Ranks.Sort();   //after applied ties must resort array
            return Ranks;
        }

        private static void ApplyTies(List<CompetitorsRankInCompetition> forTie)
        {
            if (forTie.Count > 0)
            {
                //solve Ties
                List<CompetitorsRankInCompetition> ties = TieDecision(forTie);
                foreach (CompetitorsRankInCompetition t in ties)
                {
                    foreach (CompetitorsRankInCompetition r in forTie)
                    {
                        if (t.competitor.Equals(r.competitor))
                        {
                            r.SetRank(t.rankInCompetition);
                            break;
                        }
                    }
                }
            }
            forTie.Clear();
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

        //public for unit test
        public static List<CompetitorsRankInCompetition> TieDecision(List<CompetitorsRankInCompetition> compeitorsForTies)
        {

            List<Competitor> competitors = new List<Competitor>();
            foreach (CompetitorsRankInCompetition cr in compeitorsForTies)
                competitors.Add(cr.competitor);

            List<CompetitionRules.CompetitorIntegerForTie> ties = new List<CompetitorIntegerForTie>();

            int resultsAsInteger = 0;
            foreach (Competitor c in competitors)
            {
                int dec = (int)Math.Pow((double)10, (double)c.RaceResults.Count - 1);
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    resultsAsInteger += cr.RaceRank * dec;
                    dec /= 10;
                }
                ties.Add(new CompetitorIntegerForTie(c, resultsAsInteger));
                resultsAsInteger = 0;
            }

            IEnumerable<CompetitorIntegerForTie> sortedTies = ties.OrderBy(i => i.raceResultsInteger);
            int rank = compeitorsForTies[0].rankInCompetition;  //now they have same rank in this array as first
            int offset = 0;
            List<CompetitorsRankInCompetition> result = new List<CompetitorsRankInCompetition>();

            foreach (CompetitorIntegerForTie ct in sortedTies)
            {
                // find ct.competitor in competitorsForTies and assign rank
                foreach (CompetitorsRankInCompetition cr in compeitorsForTies)
                {
                    if (cr.competitor.Equals(ct.competitor))
                    {
                        result.Add(new CompetitorsRankInCompetition(ct.competitor,rank + offset++));
                        //cr.SetRank(rank + offset++);
                        break;
                    }
                }
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
