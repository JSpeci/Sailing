using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    /* Simple class for loading competitors*/
    class Competitors
    {
        private List<Competitor> competitors;
        private String[] pathsToCsv;

        public Competitors(String[] pathsToCsv)
        {
            this.pathsToCsv = pathsToCsv;
            this.competitors = new List<Competitor>();
            loadCompetitorsNames();       //procedure
        }

        public List<Competitor> CompetitorsList { get => competitors; }

        private void loadCompetitorsNames()
        {
            String[] allLines;
            try
            {
                allLines = File.ReadAllLines(pathsToCsv[0]);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(message: "Invalid path to csv file");
            }

            /* We have only one separator for row */
            char[] separator = new char[1];
            separator[0] = ',';

            for (int x = 1; x < allLines.Length; x++)
            {

                //Splitted by separator ','
                String[] row = allLines[x].Split(separator, 2);

                /* Validate data, columns must be non-empty */
                String name = row[0];
                Exception ex = new InvalidDataException("Invalid data format in csv 2.collumn " + (x + 1) + ".row. Expected positive number, or name of competitor was not founded");

                /* Set of validation conditions for columns*/
                bool validatedRow = true;
                validatedRow &= (row[0] != String.Empty);
                validatedRow &= (row[1] != String.Empty);
                validatedRow &= (name.Length < 30);

                /* Validated row can be inserted to raceResult */
                if (validatedRow)
                {
                    competitors.Add(new Competitor(name));
                }
                else
                {
                    throw ex;
                }
            }
        }
    }

}
