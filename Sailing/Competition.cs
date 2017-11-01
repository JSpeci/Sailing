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
        private List<CompetitorsRankInCompetition> ranks;   //ranks of competitors in this competition
        private int discards = 1;     //For discards n=1 (the worst race shouldn't be taken into account).  
 

        public List<CompetitorsRankInCompetition> Ranks { get => ranks; }
        public int Discards { get => discards; }

        public Competition(List<Competitor> competitors, List<Race> races)
        {
            this.competitors = competitors;
            this.races = races;

            sumPoints();                //procedure
            computeRanks();             //procedure
        }

        

        private void computeRanks()
        {

            competitors.Sort(); //competitors sorted by points

            this.ranks = new List<CompetitorsRankInCompetition>(competitors.Count);

            /* Temporary arrays for computing ranks from points*/
            int[] rankArray = new int[competitors.Count];
            float[] pointsArray = new float[competitors.Count];
            int index = 0;
            foreach (Competitor c in competitors)
            {
                pointsArray[index] = c.Points;
                index++;
            }

            int iter = 1;
            float previous = -1F;
            for(int x = 0; x < pointsArray.Length; x++)
            {
                if(previous == pointsArray[x])
                {
                    rankArray[x] = rankArray[x - 1];
                }
                else
                {
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
        private void sumPoints()
        {
            // Discards !! n = 1

            foreach(Competitor c in this.competitors)
            {
                //left out n=1 worst races

                float sum = 0;
                int racesCount = c.RaceResults.Count;

                /*c.RaceResults is sorted list. Sort method called when each CompetitorResult object added in Race method loadDataFromCsv*/
                foreach (CompetitorResult cr in c.RaceResults)
                {
                    if (racesCount != this.discards)
                    {
                        //the result of competition is simple a sum of race points for each competitor
                        sum += cr.PointsInRace;
                    }
                    racesCount--;
                }
                c.AddPoints(sum);
            }
        }

        /* Console output string */
        public string ViewTable()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("|{0,8}|{1,8}|{2,8}|", "Name", "Rank", "Points"));
            sb.Append("\n");
            foreach (CompetitorsRankInCompetition cr in Ranks)
            {        
                sb.Append(String.Format("|{0,8}|{1,8}|{2,8}|", cr.competitor.Name, cr.rankInCompetition.ToString(), cr.competitor.Points.ToString()));
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
