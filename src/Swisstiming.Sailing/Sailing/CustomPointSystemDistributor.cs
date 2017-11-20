using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public class CustomPointSystemDistributor : IScoreDistributor
    {
        public CustomPointSystemDistributor() { }
        public readonly int[] scoringScale = { 10, 7, 5, 4, 3, 2, 1 };

        /*
            Vyrobit nastavitelny system bodovani raců tak, ze nyní je to prostý linearni low-point system
            finishing time - 1 2 3 3 4 5 
            points ted  1 2 3 4 5 6 

            nove points 10,7,5,4,3,2,1,0,0,0,0,0,0  - custom řada ze zadani
            V competition! otočit sortovani - na prvnim miste v poli ranks je ten co ma nejvice bodu - pravidla pro stejny pocet bodu zustavaji

            Udelat to rozsiritelne  - nastaveni bude nejaky objekt s pravidly - bude mozne pouzit obe pravidla, ale vzdy na celou Competition

        */

        public void ComputePointsAndRanks(List<CompetitorResult> raceResult)
        {
            
            /*Computed on temporary float arrays */
            int[] positionArray = new int[raceResult.Count];    //array of positions from csv as competitors finished
            float[] pointsResult = new float[raceResult.Count];     //computed points
            int[] rankArray = new int[raceResult.Count];            //computed rank in one race

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
            float points = (float)CustomScoringScale(1);
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
                    points += CustomScoringScale(x+1);
                    rankArray[x] = rank - countOfSame;
                    countOfSame++;

                }
                else
                {
                    countOfSame = 1;
                    points = CustomScoringScale(x+1); //x is indexing value of array, BUT parameter of method is order
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

        private int CustomScoringScale(int positionFinished)
        {
            if(positionFinished >= 1 && positionFinished <= 7)  // 7 positions in scoring scale
            {
                return scoringScale[positionFinished - 1];
            }
            else
            {
                return 0;
            }
        }
    }
}
