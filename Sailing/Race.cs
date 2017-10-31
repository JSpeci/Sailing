using System;
using System.Collections.Generic;
using System.IO;

namespace Sailing
{
    /* Race object represents one race and positions of competitors*/
    class Race
    {
        private int numberOfCompetitors;

        private List<CompetitorResult> raceResult;

        private String pathToCsv;   //for ToString method

        private List<Competitor> competitors;  //Race holds reference for list of competitors in Competition



        public int NumberOfCompetitors
        {
            get { return this.numberOfCompetitors; }
        }

        public List<CompetitorResult> RaceResult
        {
            get { return this.raceResult; }
        }

        
        public Race(String pathToCsv, List<Competitor> competitors)
        {
            this.competitors = competitors;
            this.pathToCsv = pathToCsv;
            this.numberOfCompetitors = competitors.Count;
            this.raceResult = new List<CompetitorResult>(competitors.Count);  //predicted capacity

            loadDataFromCsv(pathToCsv);   //procedure for load 
            computePointsAndRanks();    //procedure for compute points 

        }
            
        private void loadDataFromCsv(String path)
        {
            /* try to read file */
            String[] allLines;
            try
            {
                allLines = File.ReadAllLines(path);
            }
            catch(FileNotFoundException e)
            {
                throw new FileNotFoundException(message: "Invalid path to csv file");
            }

            /* We have only one separator for row */
            char[] separator = new char[1];
            separator[0] = ',';

            /* First line of csv are names of columns, so x = 1 */
            for (int x = 1; x < allLines.Length; x++)
            {
                //Splitted by separator ','
                String[] row = allLines[x].Split(separator, 2);

                /* Validate data, columns must be non-empty and second column must be positive number */
                int position;
                String name = row[0];
                Competitor competitor = findfCompetitorByName(name);
                Exception ex = new InvalidDataException("Invalid data format in csv 2.collumn " + (x + 1) + ".row. Expected positive number, or name of competitor was not founded");

                /* Set of validation conditions for columns*/
                bool validatedRow = true;
                validatedRow &= int.TryParse(row[1], out position);
                validatedRow &= (position > 0);
                validatedRow &= (competitor != null);

                /* Validated row can be inserted to raceResult */
                if (validatedRow)
                {
                    CompetitorResult cr = new CompetitorResult(competitor, position, this);
                    raceResult.Add(cr);
                    competitor.MyResults.Add(cr);
                    competitor.MyResults.Sort();
                }
                else
                {
                    throw ex;
                }
            }
        }

        /* after each race competitors position is assigned in order they finished (1, 2, 3, ..) it can happen they finished at the same time (1, 1, 2, 3)!
            the points are assigned in the same order as position based on the pointing system, the simples point system is (1, 2, 3, 4, ...)
            if some of the competitors finished on the same position the points are splitted between them (position: 1, 1, 2; points: 1.5, 1.5, 3)
            */
        private void computePointsAndRanks()
        {
            raceResult.Sort(); //simple sort based on position

            /*Ties rules implementation*/

            /*if some of the competitors finished on the same position the points are splitted between them*/

            /* the rank is assigned based on points if some of the competitors has the same number of points,
             * they have the same rank but the next rank is left out (points: 1.5, 1.5, 3, 4, rank: 1, 1, 3, 4) */

            float[] positionArray = new float[this.numberOfCompetitors];
            float[] pointsResult = new float[this.numberOfCompetitors];
            int[] rankArray = new int[this.numberOfCompetitors];

            int index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                positionArray[index] = (float)cr.PositionFinished;
                index++;
            }

            float points = 1;
            pointsResult[0] = points;
            rankArray[0] = 1;
            int countOfSame = 1;
            float iter = 2;
            for (int x = 1; x < positionArray.Length; x++)
            {
                if (positionArray[x - 1] == positionArray[x])
                {
                    points += iter;
                    countOfSame++;
                    rankArray[x] = ((int)iter)-countOfSame;
                }
                else
                {
                    countOfSame = 1;
                    points = iter;
                    rankArray[x] = (int)iter;
                }
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

            index = 0;
            foreach (CompetitorResult cr in raceResult)
            {
                cr.Points = pointsResult[index];
                cr.RaceRank = rankArray[index];
                index++;
            }

        }

        private Competitor findfCompetitorByName(String name)
        {
            foreach(Competitor c in competitors)
            {
                if(String.Equals(c.Name,name))
                {
                    return c;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return "Race from: " + this.pathToCsv + " Count: " + this.numberOfCompetitors;
        }

    }
}
