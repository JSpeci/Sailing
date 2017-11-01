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

            //competitors array
            CompetitorsLoader competitorsLoader = new CompetitorsLoader(fileArray);

            //one instance of competition
            Competition competition = new Competition(competitorsLoader.Competitors,competitorsLoader.Races);

            //output
            Console.WriteLine(competition.ViewTable());

            Console.ReadLine();
        }
    }
}
