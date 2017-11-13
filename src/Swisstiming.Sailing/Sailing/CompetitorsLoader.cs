using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sailing
{
    public static class CompetitorsLoader
    {
        public static Competition LoadCompetitorsFromDirectoryPath(string dirPath)
        {
            // data structures for competition
            List<Competitor> Competitors = new List<Competitor>();
            List<Race> Races = new List<Race>();

            //array of paths for CSVs
            string[] fileArray = System.IO.Directory.GetFiles(dirPath, "*.csv");

            //there are multiple races (2-16) in a single competition
            if (fileArray.Length < 2 || fileArray.Length > 16)
            {
                throw new Exception("Count of csv files is not in range 2-16, depends on rules.");
            }

            /* We have only one separator for row in CSv*/
            char[] separator = new char[1];
            separator[0] = ',';

            foreach (string csvPath in fileArray)
            {
                List<CompetitorResult> raceResult = new List<CompetitorResult>();

                /* try to read file */
                string[] allLines;
                try
                {
                    allLines = File.ReadAllLines(csvPath);
                }
                catch
                {
                    throw new FileNotFoundException(message: "Invalid path to csv file");
                }

                /* First line of csv are names of columns, so x = 1 */
                for (int x = 1; x < allLines.Length; x++)
                {
                    //Splitted by separator ','
                    string[] row = allLines[x].Split(separator, 2);

                    /* Validate data, columns must be non-empty and second column must be positive number */
                    int position;
                    string name = row[0];
                    Competitor competitor = FindfCompetitorByName(Competitors, name);
                    Exception exception = new InvalidDataException("Invalid data format in csv 2.collumn " + (x + 1) + ".row. Expected positive number, or name of competitor was not founded");

                    /* Set of validation conditions for columns*/
                    bool validatedRow = true;
                    validatedRow &= (row[0] != string.Empty);
                    validatedRow &= (row[1] != string.Empty);
                    validatedRow &= (name.Length < 30);
                    validatedRow &= int.TryParse(row[1], out position);
                    validatedRow &= (position > 0);

                    /*If competitor with name doesnt exists in list of competitors, should be added*/
                    if (competitor == null)
                    {
                        competitor = new Competitor(name);
                        Competitors.Add(competitor);
                    }

                    /* Validated row can be inserted to raceResult */
                    if (validatedRow)
                    {
                        CompetitorResult cr = new CompetitorResult(competitor, position);
                        raceResult.Add(cr);
                        competitor.RaceResults.Add(cr);
                        competitor.RaceResults.Sort();
                    }
                    else
                    {
                        throw exception;
                    }
                }



                Race race = new Race(raceResult);
                Races.Add(race);

            }


            //returned competition
            Competition c = new Competition(Competitors, Races);
            return c;
        }

        /*Returns reference to Competitor identified by name*/
        private static Competitor FindfCompetitorByName(List<Competitor> Competitors, string name)
        {
            foreach (Competitor c in Competitors)
            {
                if (string.Equals(c.Name, name))
                {
                    return c;
                }
            }
            return null;
        }
    }
}