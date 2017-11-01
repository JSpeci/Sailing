using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Class represents one competitor in competition */ 
namespace Sailing
{
    class Competitor : IComparable<Competitor>
    {
        private string name;
        private float points;
        private List<CompetitorResult> raceResults;

        public List<CompetitorResult> RaceResults { get => raceResults;  }
        public string Name { get => name; set => name = value; }
        public float Points { get => points; }

        public Competitor(string name)
        {
            this.raceResults = new List<CompetitorResult>();
            this.name = name;
            this.points = 0F;
        }

        /* Adding points to atrribute points. In foreach can not be modified property points. */
        public void AddPoints(float value)
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

        /* Competitor can be compared on points, sorted competitors can be ranked in all competition - many races in competition*/
        public int CompareTo(Competitor other)
        {
            return this.points.CompareTo(other.Points);
        }
    }
}
