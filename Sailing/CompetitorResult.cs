using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    /* Simple structure CompetitorResult holds one Competitor and his position in race, reference to race, and calculated rank */
    class CompetitorResult : IComparable<CompetitorResult>
    {
        private Competitor comp;
        private int positionFinished;
        private float points;
        private Race race;
        private int raceRank;

        public int PositionFinished
        {
            get { return positionFinished; }
        }
        public float Points
        {
            get { return positionFinished; }
            set { points = value; }
        }

        public Competitor Comp { get { return comp; } }

        public int RaceRank { get => raceRank; set => raceRank = value; }

        public CompetitorResult(Competitor comp, int position, Race race)
        {
            this.comp = comp;
            this.race = race;
            this.positionFinished = position;
            this.points = 0F;
        }

        public int CompareTo(CompetitorResult other)
        {
            return this.positionFinished.CompareTo(other.PositionFinished);
        }
    }
}
