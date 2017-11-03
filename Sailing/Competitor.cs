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
        private List<CompetitorResult> raceResults;

        public List<CompetitorResult> RaceResults { get => raceResults; }
        public string Name { get; set; }
        public float NetPoints { get; set; }
        public float TotalPoints { get; set; }

        public Competitor(string name)
        {
            this.raceResults = new List<CompetitorResult>();
            this.Name = name;
        }

        public override string ToString()
        {
            return "Competitor: " + Name;
        }

        /* Competitor can be compared on points, sorted competitors can be ranked in all competition - many races in competition*/
        public int CompareTo(Competitor other)
        {
            return this.NetPoints.CompareTo(other.NetPoints);
        }
        
        
    }
}
