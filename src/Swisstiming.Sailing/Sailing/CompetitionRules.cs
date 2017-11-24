using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sailing
{
    public class CompetitionRules
    {

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
            previous = 0;
            foreach (CompetitorsRankInCompetition cr in Ranks)
            {
                if (cr.rankInCompetition == previous)
                {
                    forTie.Add(cr);
                }
                else
                {
                    TieDecision(forTie);
                    //after Tie solution - clear temp array
                    forTie.Clear();
                }
                previous = cr.rankInCompetition;
            }

            return Ranks;
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
        public static void TieDecision(List<CompetitorsRankInCompetition> compeitorsForTies)
        {
            //temporary array for computing values
            //first index - competitor,
            int[,] ranks = new int[compeitorsForTies.Count, compeitorsForTies[0].competitor.RaceResults.Count];
            /*
                R1 R2 R3 R4
            C1
            C2
            C3 
            
             */

            //podívám se na jejich nejlepší závod, a vyberu toho, čí nejlepší závod byl lepší -
            int x = 0;
            int y = 0;
            foreach (CompetitorsRankInCompetition cRank in compeitorsForTies)
            {
                foreach(CompetitorResult cr in cRank.competitor.RaceResults)
                {
                    ranks[x, y] = cr.RaceRank;
                    y++;
                }
                x++;
            }


            for(x = 0; x < ranks.Length; x++)
            {
                
            }

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
