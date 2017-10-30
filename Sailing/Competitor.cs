using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Class represents one competitor in race
 *  
 **/ 
namespace Sailing
{
    class Competitor
    {
        private String nameOfTeam;

        public String NameOfTeam
        {
            get { return this.nameOfTeam; }
            set { this.nameOfTeam = value; }
        }

        public Competitor(String name)
        {
            this.nameOfTeam = name;
        }

        public override string ToString()
        {
            return "Competitor: " + nameOfTeam;
        }

    }
}
