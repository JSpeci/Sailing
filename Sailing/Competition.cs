﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    class Competition
    {
        public struct CompetitorRank
        {
            public Competitor competitor;
            public int rank;
            public CompetitorRank(Competitor c, int rank)
            {
                this.competitor = c;
                this.rank = rank;
            }
        }

        private List<Competitor> competitors;
        private List<Race> races;
        private List<CompetitorRank> ranks;

        /* For discards n=1 (the worst race shouldn't be taken into account).  */
        private int discards = 1;

        private String[] pathsToCsv;

        public Competition(String[] pathsToCsv, List<Competitor> competitors)
        {
            races = new List<Race>();
            this.competitors = competitors;
            this.pathsToCsv = pathsToCsv;

            //there are multiple races (2-16) in a single competition
            if(pathsToCsv.Length < 2 || pathsToCsv.Length > 16)
            {
                throw new Exception("Count of csv files is not in range 2-16, depends on rules.");
            }

            
            makeRacesFromData();        //procedure
            sumPoints();                //procedure
            computeRanks();             //procedure

        }

        

        private void makeRacesFromData()
        {
            foreach (String s in pathsToCsv)
            {
                races.Add(new Race(s, competitors));
            }
        }

        private void computeRanks()
        {
            competitors.Sort();
            this.ranks = new List<CompetitorRank>(competitors.Count);

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
            
            index = 0;
            foreach (Competitor c in competitors)
            {
                ranks.Add(new CompetitorRank(c, rankArray[index]));
                index++;
            }
        }

        private void sumPoints()
        {
            // Discards !! n = 1

            foreach(Competitor c in this.competitors)
            {
                //left out n=1 worst races

                float sum = 0;
                int myRaceCount = c.MyResults.Count;
                foreach(CompetitorResult cr in c.MyResults)
                {
                    if (myRaceCount != this.discards)
                    {
                        sum += cr.Points;
                    }
                    myRaceCount--;
                }
                c.addPoints(sum);
            }
        }


        public String viewTable()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("|{0,8}|{1,8}|{2,8}|", "Name", "Rank", "Points"));
            sb.Append("\n");
            foreach (CompetitorRank cr in ranks)
            {        
                sb.Append(String.Format("|{0,8}|{1,8}|{2,8}|", cr.competitor.Name, cr.rank.ToString(), cr.competitor.Points.ToString()));
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
