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
        private Competitor competitor;    //copmetitor reference
        private int positionFinished;  //position competitor finished in one race
        private float pointsInRace;       //computed points in that race
        private int raceRank;       //rank given - computed in one race

        public int PositionFinished  { get => positionFinished; }
        public Competitor Competitor { get => competitor; }
        public int RaceRank { get => raceRank; set => raceRank = value; }
        public float PointsInRace { get => pointsInRace; set => pointsInRace = value; }

        public CompetitorResult(Competitor competitor, int position)
        {
            this.competitor = competitor;
            this.positionFinished = position;
            this.pointsInRace = 0F;
        }

        /* Comparable to be sort. In sorted array can be computed points and rank in race*/
        public int CompareTo(CompetitorResult other)
        {
            return this.positionFinished.CompareTo(other.PositionFinished);
        }
    }
}
