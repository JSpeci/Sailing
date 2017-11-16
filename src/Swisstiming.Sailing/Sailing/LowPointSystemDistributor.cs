using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public class LowPointSystemDistributor : IScoreDistributor
    {
        public LowPointSystemDistributor() { }

        public void ComputePointsAndRanks(List<CompetitorResult> raceResult)
        {
            /* after each race competitors position is assigned in order they finished (1, 2, 3, ..) it can happen they finished at the same time (1, 1, 2, 3)!
            the points are assigned in the same order as position based on the pointing system, the simples point system is (1, 2, 3, 4, ...)
            if some of the competitors finished on the same position the points are splitted between them (position: 1, 1, 2; points: 1.5, 1.5, 3)
            */

            /* Working on sorted competitors sorted by position/time finished in race */
            raceResult.Sort();

            Console.WriteLine();
            /* Ties rules implementation*/

            /* if some of the competitors finished on the same position the points are splitted between them*/

            /* the rank is assigned based on points if some of the competitors has the same number of points,
             * they have the same rank but the next rank is left out (points: 1.5, 1.5, 3, 4, rank: 1, 1, 3, 4) */

            /*Computed on temporary float arrays */
            int[] positionArray = new int[raceResult.Count];    //array of positions from csv as competitors finished
            float[] pointsResult = new float[raceResult.Count];     //computed points
            int[] rankArray = new int[raceResult.Count];            //computed rank in one race

            //Working on sorted array of CompetitorResult
            //filling array of positions from data structure
            int index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                positionArray[index] = cr.PositionFinished;
                index++;
            }


            //initial values
            float points = 1;
            pointsResult[0] = points;   //first filled manually
            rankArray[0] = 1;           //first filled manually
            int countOfSame = 1;
            float iter = 2;             //iter counts rank points for single position competitors
            for (int x = 1; x < positionArray.Length; x++)
            {
                if (positionArray[x - 1] == positionArray[x])
                {
                    points += iter;     //sum of points will be divided into comps with same position
                    rankArray[x] = ((int)iter) - countOfSame;
                    countOfSame++;

                }
                else
                {
                    countOfSame = 1;
                    points = iter;
                    rankArray[x] = (int)iter;
                }
                //if I had many same values, back going for recompute points divided by count of same
                if (countOfSame > 1)
                {
                    for (int y = x; y > (x - countOfSame); y--)
                    {
                        pointsResult[y] = points / countOfSame;
                    }

                }
                else
                {
                    pointsResult[x] = points;
                }
                iter++;
            }

            //refilling back to data structure of CompetitorResults
            index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                cr.PointsInRace = pointsResult[index];
                cr.RaceRank = rankArray[index];
                index++;
            }
        }
    }
}
