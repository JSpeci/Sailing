using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public class RaceRules
    {
        public static void ComputePointsAndRanks(Race race, IPointSystem pointSystem)
        {
            List<CompetitorResult> raceResult = race.RaceResult;
            /*Computed on temporary float arrays */
            int[] positionArray = new int[raceResult.Count];         //array of positions from csv as competitors finished
            float[] pointsResult = new float[raceResult.Count];      //computed points
            int[] rankArray = new int[raceResult.Count];             //computed rank in one race

            //Working on sorted array of CompetitorResult
            //filling array of positions from data structure
            raceResult.Sort();
            int index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                positionArray[index] = cr.PositionFinished;
                index++;
            }

            //first filled manually
            float points = (float)pointSystem.GetPointsFromPosition(1);
            pointsResult[0] = points;

            int countOfSame = 1;    //counter of same position in race

            //first filled manually
            int rank = 1;
            rankArray[0] = 1;
            rank++;

            for (int x = 1; x < positionArray.Length; x++)
            {
                if (positionArray[x - 1] == positionArray[x])
                {
                    //x is indexing value of array, BUT parameter of method is order
                    points += pointSystem.GetPointsFromPosition(x + 1);
                    rankArray[x] = rank - countOfSame;
                    countOfSame++;

                }
                else
                {
                    countOfSame = 1;
                    //x is indexing value of array, BUT parameter of method is order
                    points = pointSystem.GetPointsFromPosition(x + 1);
                    rankArray[x] = rank;
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
                rank++;
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
