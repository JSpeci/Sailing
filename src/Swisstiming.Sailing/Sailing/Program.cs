using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Sailing
{
    public class Program
    {



        static void Main(string[] args)
        {

            //one instance of competition
            Competition competition = CompetitorsLoader.LoadCompetitorsFromDirectoryPath("./data1/");
            //competition.ApplyRules(new CustomPointSystem());
            competition.ApplyDiscards(2);
            //output
            Console.WriteLine(CompetitionViewer.ViewTable(competition));


            Console.ReadLine();

        }
    }
}
