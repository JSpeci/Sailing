using System;

namespace Sailing
{
    /* CompetitorResult holds one Competitor and his position in ONE race, reference to race, and calculated rank */
    public class CompetitorResult : IComparable<CompetitorResult>
    {
        // auto property https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties
        // Auto-Impl Properties for trivial get and set
        public Competitor Competitor { get; }    //copmetitor reference
        public int PositionFinished  { get; } //position competitor finished in one race
        public int RaceRank { get; set; } //rank given - computed in one race
        public float PointsInRace { get; set; } //computed points in that race
        public bool Discarded { get; set; }

        public CompetitorResult(Competitor competitor, int position)
        {
            this.Competitor = competitor;
            this.PositionFinished = position;
            this.PointsInRace = 0F;
            this.Discarded = false;
        }

        /* Comparable to be sort by position finished. In sorted array can be computed points and rank in race*/
        public int CompareTo(CompetitorResult other)
        {
            return this.PositionFinished.CompareTo(other.PositionFinished);
        }
    }
}
