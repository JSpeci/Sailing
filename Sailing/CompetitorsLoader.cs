﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    /* Simple class for loading competitors by names from one csv file*/
    class CompetitorsLoader
    {
        public List<Competitor> Competitors { get; }
        public List<Race> Races { get; }

        public CompetitorsLoader()
        {
            Competitors = new List<Competitor>();
            Races = new List<Race>();
        }

        public void LoadCompetitorsRaces(string[] csvPaths)
        {
            MakeRacesFromData(csvPaths);
        }


        /* Loops through CSV files and makes races objects*/
        private void MakeRacesFromData(string[] csvPaths)
        {
            //there are multiple races (2-16) in a single competition
            if (csvPaths.Length < 2 || csvPaths.Length > 16)
            {
                throw new Exception("Count of csv files is not in range 2-16, depends on rules.");
            }

            foreach (string s in csvPaths)
            {
                Races.Add(new Race(LoadDataFromCsv(s)));
            }
        }

        /* Loads names and positions from csv */
        private  List<CompetitorResult> LoadDataFromCsv(string csvPath)
        {

            List<CompetitorResult> raceResult = new List<CompetitorResult>();
            /* try to read file */
            string[] allLines;
            try
            {
                allLines = File.ReadAllLines(csvPath);
            }
            catch (FileNotFoundException e)
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
                string[] row = allLines[x].Split(separator, 2);

                /* Validate data, columns must be non-empty and second column must be positive number */
                int position;
                string name = row[0];
                Competitor competitor = FindfCompetitorByName(name);
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
            return raceResult;
        }

        /*Returns reference to Competitor identified by name*/
        private Competitor FindfCompetitorByName(string name)
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
