using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public interface IScoreDistributor
    {
        void ComputePointsAndRanks(List<CompetitorResult> raceResult);
    }
}
