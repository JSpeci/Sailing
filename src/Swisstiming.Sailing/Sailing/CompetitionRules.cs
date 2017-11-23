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
            competition.Ranks = ComputeRanks(competition.Competitors, pointSystem.DescendingPoints());
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

        public static List<CompetitorsRankInCompetition> ComputeRanks(List<Competitor> competitors, bool descendingPoints)
        {
            IEnumerable<Competitor> sortedCompetitors;


            if (descendingPoints)
            {
                sortedCompetitors = competitors.OrderByDescending(p => p.NetPoints);
            }
            else
            {
                sortedCompetitors = competitors.OrderBy(p => p.NetPoints);
            }

            List<CompetitorsRankInCompetition> Ranks = new List<CompetitorsRankInCompetition>();

            /* Temporary arrays for computing ranks from points*/
            int[] rankArray = new int[competitors.Count];
            float[] pointsArray = new float[competitors.Count];
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
                
                previous = pointsArray[x];
                
            }

            /*Refilling data structures from temporary arrays*/
            index = 0;
            foreach (Competitor c in sortedCompetitors)
            {
                Ranks.Add(new CompetitorsRankInCompetition(c, rankArray[index]));
                index++;
            }

            return Ranks;
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
