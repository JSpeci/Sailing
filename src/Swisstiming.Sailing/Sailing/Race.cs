using System;
using System.Collections.Generic;
using System.IO;

namespace Sailing
{
    /* Race object represents one race and positions of competitors  
       Loads data from one csv
       Build data structures
       Compute points and ranks in one race
    */
    public class Race
    {
        public List<CompetitorResult> RaceResult { get; } //List of competitors and his points, rank


        public Race(List<CompetitorResult> raceResult)
        {
            this.RaceResult = raceResult; 
        }

        public void ComputePointsAndRanks(IScoreDistributor distributor)
        {
            distributor.ComputePointsAndRanks(this.RaceResult);
        }

    }
}
