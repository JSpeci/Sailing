using System;
using System.Collections.Generic;
using System.IO;

namespace Sailing
{
    /* Race object represents one race and positions of competitors*/
    class Race
    {
        /* Simple structure CompetitorResult holds one Competitor and his position in race*/
        public struct CompetitorResult
        {
            public Competitor competitor;
            public int positionFinished;
            public CompetitorResult(Competitor comp, int position)
            {
                this.competitor = comp;
                this.positionFinished = position;
            }
        }


        private int numberOfCompetitors;
        private List<CompetitorResult> raceResult;

        private String pathToCsv;   //for ToString method

        public int NumberOfCompetitors
        {
            get { return this.numberOfCompetitors; }
        }

        public List<CompetitorResult> RaceResult
        {
            get { return this.raceResult; }
        }

        
        public Race(String pathToCsv)
        {
            this.pathToCsv = pathToCsv;
            this.raceResult = new List<CompetitorResult>();
            loadDataFromCsv(pathToCsv);                         //procedure
            this.numberOfCompetitors = this.raceResult.Count;   
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
                Exception ex = new InvalidDataException("Invalid data format in csv 2.collumn " + (x + 1) + ".row. Expected positive number");

                /* Set of validation conditions for columns*/
                bool validatedRow = true;
                validatedRow &= (row[0] != String.Empty);
                validatedRow &= (row[1] != String.Empty);

                validatedRow &= int.TryParse(row[1], out position);
                validatedRow &= (position > 0);
                validatedRow &= (name.Length < 30);

                /* Validated row can be inserted to raceResult */
                if(validatedRow)
                {
                     raceResult.Add(new CompetitorResult(new Competitor(name), position)); 
                }
                else
                {
                    throw ex;
                }
            }
        }
        public override string ToString()
        {
            return "Race from: " + this.pathToCsv + " Count: " + this.numberOfCompetitors;
        }

    }
}
