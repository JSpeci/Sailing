using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    public class Competition
    {

        public List<CompetitorsRankInCompetition> Ranks { get; set; } //ranks of competitors in this competition 

        public int Discards { get; set; }   //For discards n=1 (the worst race shouldn't be taken into account). 

        public List<Race> Races { get; private set; }

        public List<Competitor> Competitors { get; private set; }

        public IPointSystem PointSystem { get; set; }

        public Competition(List<Competitor> competitors, List<Race> races)
        {
            Competitors = competitors;
            Races = races;
            Discards = 1;
        }
    }
}