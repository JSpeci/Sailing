using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    class Competition
    {

        private List<Competitor> competitors;
        private List<Race> races;

        /* For discards n=1 (the worst race shouldn't be taken into account).  */
        private int discards = 1;

        private String[] pathsToCsv;

        public Competition(String[] pathsToCsv, List<Competitor> competitors)
        {
            races = new List<Race>();
            this.competitors = competitors;
            this.pathsToCsv = pathsToCsv;

            //there are multiple races (2-16) in a single competition
            if(pathsToCsv.Length < 2 || pathsToCsv.Length > 16)
            {
                throw new Exception("Count of csv files is not in range 2-16, depends on rules.");
            }

            
            makeRacesFromData();        //procedure
            sumPoints();                //procedure
            computeRanks();             //procedure

        }

        

        private void makeRacesFromData()
        {
            foreach (String s in pathsToCsv)
            {
                races.Add(new Race(s, competitors));
            }
        }

        private void computeRanks()
        {

        }

        private void sumPoints()
        {
            // Discards !! n = 1

            foreach(Competitor c in this.competitors)
            {
                //left out n=1 worst races

            }
        }


        public String viewTable()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
