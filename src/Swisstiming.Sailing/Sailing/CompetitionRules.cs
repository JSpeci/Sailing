using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sailing
{
    public class CompetitionRules
    {
        /*
         Predelat metody na public abych mohl testovat jednotlive a predem si data pripravit. 
         Parametry metod budou jen ty, ktere pouzivam, bool pro descending a pak testy.

             */
        public static void ApplyRules(Competition competition, IPointSystem pointSystem)
        {
            //deafult pointSystem
            ApplyRaceRules(competition, pointSystem);
            ApplyDiscards(competition);
            SumPoints(competition);                
            ComputeRanks(competition);             
        }

        private static void ApplyRaceRules(Competition competition, IPointSystem pointSystem)
        {
            competition.PointSystem = pointSystem;
            foreach (Race r in competition.Races)
                RaceRules.ComputePointsAndRanks(r, pointSystem);
        }

        private static void ApplyDiscards(Competition competition)
        {
            if (competition.Discards < 0)
            {
                throw new Exception("Discards must be positive number. ");
            }

            // prepared for new discards
            foreach (Competitor c in competition.Competitors)
            {
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    cr.Discarded = false;
                }
            }

            foreach (Competitor c in competition.Competitors)
            {
                int racesCount = c.RaceResults.Count;

                IEnumerable<CompetitorResult> sortedResults;
                sortedResults = c.RaceResults.OrderBy(r => r.RaceRank);

                //make discards
                foreach (CompetitorResult cr in sortedResults)
                {
                    if (racesCount <= competition.Discards)
                    {
                        cr.Discarded = true;
                    }
                    racesCount--;
                }
            }
        }
        private static void ComputeRanks(Competition competition)
        {
            IEnumerable<Competitor> sortedCompetitors;


            if (competition.PointSystem.DescendingPoints())
            {
                sortedCompetitors = competition.Competitors.OrderByDescending(p => p.NetPoints);
            }
            else
            {
                sortedCompetitors = competition.Competitors.OrderBy(p => p.NetPoints);
            }

            competition.Ranks = new List<CompetitorsRankInCompetition>();

            /* Temporary arrays for computing ranks from points*/
            int[] rankArray = new int[competition.Competitors.Count];
            float[] pointsArray = new float[competition.Competitors.Count];
            int index = 0;
            foreach (Competitor c in sortedCompetitors)
            {
                pointsArray[index] = c.NetPoints;
                index++;
            }

            int iter = 1;   //rank for competitors with different sum of points
            float previous = -1F;   //temporary variable
            for (int x = 0; x < pointsArray.Length; x++)
            {
                //if competitor has same sum of points as previous competitor, they have same rank
                if (previous == pointsArray[x])
                {
                    rankArray[x] = rankArray[x - 1];
                }
                else
                {
                    // rank assigned by order
                    rankArray[x] = iter;
                }
                iter++;
                if (x != 0)
                {
                    previous = pointsArray[x];
                }
            }

            /*Refilling data structures from temporary arrays*/
            index = 0;
            foreach (Competitor c in sortedCompetitors)
            {
                competition.Ranks.Add(new CompetitorsRankInCompetition(c, rankArray[index]));
                index++;
            }
        }
        /*
             Sum of points of every competitor, includes discards
             Goes throug every competitor
             Goes throug every recorded race in competitors list myRaces
             Sum of all points from myRaces
             Discard-omit the n last races
        */
        private static void SumPoints(Competition competition)
        {
            // Discards !! n = 1

            foreach (Competitor c in competition.Competitors)
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
