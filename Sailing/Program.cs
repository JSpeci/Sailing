using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    class Program
    {
        static void Main(string[] args)
        {

            //directory with csv files
            string[] fileArray = Directory.GetFiles("./data1/", "*.csv");

            CompetitorsLoader loader = new CompetitorsLoader();
            loader.LoadCompetitorsRaces(fileArray);

            //one instance of competition
            Competition competition = new Competition(loader.Competitors,loader.Races);

            //output
            Console.WriteLine(competition.ViewTable());

            Console.ReadLine();
        }
    }
}
