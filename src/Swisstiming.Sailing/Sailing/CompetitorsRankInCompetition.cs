using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    /* Simple struct holds pair Competitor and his rank in competition*/
    public class CompetitorsRankInCompetition : IComparable<CompetitorsRankInCompetition>
    {
        public Competitor competitor;
        public int rankInCompetition;
        public CompetitorsRankInCompetition(Competitor competitor, int rank)
        {
            this.competitor = competitor;
            this.rankInCompetition = rank;
        }
        public CompetitorsRankInCompetition()
        {
            this.competitor = null;
            this.rankInCompetition = -1;
        }
        public void SetRank(int val)
        {
            this.rankInCompetition = val;
        }

        public int CompareTo(CompetitorsRankInCompetition other)
        {
            return this.rankInCompetition.CompareTo(other.rankInCompetition);
        }
    }
}
