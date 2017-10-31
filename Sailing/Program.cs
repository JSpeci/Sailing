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

            String[] fileArray = Directory.GetFiles("./data1/", "*.csv");

            Competitors comps = new Competitors(fileArray);

            Competition c = new Competition(fileArray, comps.CompetitorsList);

            Console.WriteLine(c.viewTable());

            Console.ReadLine();
        }
    }
}
