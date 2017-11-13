using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    public class Program
    {
        static void Main(string[] args)
        {

            //one instance of competition
            Competition competition = CompetitorsLoader.LoadCompetitorsFromDirectoryPath("./data1/");

            //output
            Console.WriteLine(CompetitionViewer.ViewTable(competition));


            Console.ReadLine();

        }
    }
}
