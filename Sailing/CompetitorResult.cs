using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    /* CompetitorResult holds one Competitor and his position in ONE race, reference to race, and calculated rank */
    class CompetitorResult : IComparable<CompetitorResult>
    {
        private Competitor comp;    //copmetitor reference
        private int positionFinished;  //position competitor finished in one race
        private float points;       //computed points in that race
        private Race race;          //reference to raceObject
        private int raceRank;       //rank given - computed in one race

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

        /* Comparable to be sort. In sorted array can be computed points and rank in race*/
        public int CompareTo(CompetitorResult other)
        {
            return this.positionFinished.CompareTo(other.PositionFinished);
        }
    }
}
