using System;
using System.Collections.Generic;

namespace Sailing
{
    class Race
    {
        
        public struct CompetitorResult
        {
            public Competitor competitor;
            public int order;
        }

        private int numberOfCompetitors;
        private List<CompetitorResult> raceResult;
        

        public int NumberOfTeams
        {
            get { return this.numberOfCompetitors; }
        }

        public List<CompetitorResult> RaceResult
        {
            get { return this.raceResult; }
        }

        public Race(String pathToCsv)
        {
            this.raceResult = new List<CompetitorResult>();
            loadDataFromCsv(pathToCsv);
            this.numberOfCompetitors = this.raceResult.Count;
        }
            
        private void loadDataFromCsv(String path)
        {
            String[] allLines = System.IO.File.ReadAllLines(path);
            char[] separator = new char[1];
            separator[0] = ',';

            for (int x = 0; x < allLines.Length; x++)
            {
                String[] row = allLines[x].Split(separator, 2);
                Console.WriteLine(row[0] + " " + row[1]);
            }
        }

    }
}
