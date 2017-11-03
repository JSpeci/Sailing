using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    class Competition
    {

        /* Simple struct holds pair Competitor and his rank in competition*/
        public struct CompetitorsRankInCompetition
        {
            public Competitor competitor;
            public int rankInCompetition;
            public CompetitorsRankInCompetition(Competitor competitor, int rank)
            {
                this.competitor = competitor;
                this.rankInCompetition = rank;
            }
        }


        private List<Competitor> competitors; //competitors list reference
        private List<Race> races;             //races in this competition
        private List<CompetitorsRankInCompetition> ranks;

        public List<CompetitorsRankInCompetition> Ranks { get => ranks; } //ranks of competitors in this competition 
        public int Discards { get; set; }   //For discards n=1 (the worst race shouldn't be taken into account). 

        public Competition(List<Competitor> competitors, List<Race> races)
        {
            this.competitors = competitors;
            this.races = races;
            this.Discards = 1;

            SumPoints();                //procedure
            ComputeRanks();             //procedure
        }



        private void ComputeRanks()
        {

            competitors.Sort(); //competitors sorted by points

            this.ranks = new List<CompetitorsRankInCompetition>(competitors.Count);

            /* Temporary arrays for computing ranks from points*/
            int[] rankArray = new int[competitors.Count];
            float[] pointsArray = new float[competitors.Count];
            int index = 0;
            foreach (Competitor c in competitors)
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
            foreach (Competitor c in competitors)
            {
                Ranks.Add(new CompetitorsRankInCompetition(c, rankArray[index]));
                index++;
            }
        }

        /*Sum of points of every competitor, includes discards
          Goes throug every competitor
          Goes throug every recorded race in competitors list myRaces
          Sum of all points from myRaces
          Discard-omit the n last races
         */
        private void SumPoints()
        {
            // Discards !! n = 1

            foreach (Competitor c in this.competitors)
            {
                //left out n=1 worst races

                float sum = 0;
                float sumTotal = 0;
                int racesCount = c.RaceResults.Count;

                //make discards
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    if (racesCount <= this.Discards)
                    {
                        cr.Discarded = true;
                    }
                    racesCount--;
                }

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

        /* Console output string */
        /* Console output string */
        public string ViewTable()
        {
            StringBuilder sb = new StringBuilder();

            //Table header
            sb.Append(String.Format("{0,8}{1,8}", "Competitor", "Rank"));
            int raceNumber = 1;
            foreach (Race r in races)
            {
                sb.Append(String.Format("{0,8}","Race"+raceNumber++.ToString()));
            }
            sb.Append(String.Format("{0,8}{1,8}", "Net", "Total"));
            sb.Append("\n");

            //Table content
            foreach (CompetitorsRankInCompetition cRank in Ranks)
            {
                sb.Append(String.Format("{0,8}{1,8}", cRank.competitor.Name, cRank.rankInCompetition.ToString()));
                foreach (Race r in races)
                {
                    CompetitorResult cResTemp = null;
                    foreach(CompetitorResult cRes in r.RaceResult)
                    {
                        if(string.Equals(cRank.competitor.Name,cRes.Competitor.Name))
                        {
                            cResTemp = cRes;
                        }
                    }
                    sb.Append(String.Format("{0,8}", cResTemp.Discarded ? ("(" + cResTemp.PointsInRace.ToString() + ")") : cResTemp.PointsInRace.ToString()));
                }
                sb.Append(String.Format("{0,8}{1,8}", cRank.competitor.NetPoints.ToString(), cRank.competitor.TotalPoints.ToString()));
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
