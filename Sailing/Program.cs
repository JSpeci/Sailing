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

            Competition c = new Competition(fileArray);

            Console.WriteLine(c.viewTable());

            Console.ReadLine();
        }
    }
}
