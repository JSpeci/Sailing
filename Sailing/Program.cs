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
            String[] fileArray = Directory.GetFiles("./data1/", "*.csv");

            //competitors array
            Competitors comps = new Competitors(fileArray);

            //one instance of competition
            Competition c = new Competition(fileArray, comps.CompetitorsList);

            //output
            Console.WriteLine(c.viewTable());

            Console.ReadLine();
        }
    }
}
