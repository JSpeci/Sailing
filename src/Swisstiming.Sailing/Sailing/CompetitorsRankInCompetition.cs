using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    /* Simple struct holds pair Competitor and his rank in competition*/
    public struct CompetitorsRankInCompetition
    {
        public Competitor competitor;
        public int rankInCompetition;
        public CompetitorsRankInCompetition(Competitor competitor, int rank)
        {
            this.competitor = competitor;
            this.rankInCompetition = rank;
        }
    }
}
