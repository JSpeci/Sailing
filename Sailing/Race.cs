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
            computePoints();    //procedure for compute points 

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
                Exception ex = new InvalidDataException("Invalid data format in csv 2.collumn " + (x + 1) + ".row. Expected positive number, or name of competitor was not founded");

                /* Set of validation conditions for columns*/
                bool validatedRow = true;
                validatedRow &= int.TryParse(row[1], out position);
                validatedRow &= (position > 0);
                validatedRow &= (findfCompetitorByName(name) != null);

                /* Validated row can be inserted to raceResult */
                if (validatedRow)
                {
                     raceResult.Add(new CompetitorResult(findfCompetitorByName(name), position)); 
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void computePoints()
        {
            foreach(CompetitorResult cr in raceResult)
            {
                cr.Points = 1F;
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
