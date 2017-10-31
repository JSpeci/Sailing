using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** Class represents one competitor in race,
 *  now has only name.
 **/ 
namespace Sailing
{
    class Competitor
    {
        private String name;
        private float points;
        private List<CompetitorResult> myResults;

        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public float Points
        {
            get { return this.points; }
        }

        public List<CompetitorResult> MyResults { get => myResults;  }

        public Competitor(String name)
        {
            this.myResults = new List<CompetitorResult>();
            this.name = name;
            points = 0F;
        }

        public void addPoints(float value)
        {
            if (value > 0)
            {
                this.points += value;
            }
            else throw new InvalidOperationException("Negative value can not be added.");
        }
        public override string ToString()
        {
            return "Competitor: " + name;
        }
        
        
    }
}
