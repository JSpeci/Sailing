using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public interface IScoreDistributor
    {
        void ComputePoints(List<CompetitorResult> raceResult);
        void ComputeRanks(List<CompetitorResult> raceResult);
    }
}
