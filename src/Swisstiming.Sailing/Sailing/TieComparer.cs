using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public class TieComparer : IComparer<CompetitorsRankInCompetition>
    {
        /*
              Less than zero 
                This instance precedes obj in the sort order. 
              Zero 
                This instance occurs in the same position in the sort order as obj. 
              Greater than zero 
                This instance follows obj in the sort order. 
        */
        public int Compare(CompetitorsRankInCompetition x, CompetitorsRankInCompetition y)
        {
            if (x.competitor.SumOfRanks == y.competitor.SumOfRanks)
            {
                return CompareRaceResult(x, y, 0);
            }
            else
            {
                return x.competitor.SumOfRanks.CompareTo(y.competitor.SumOfRanks);
            }
        }

        //compare two race result recursively by raceResult
        private int CompareRaceResult(CompetitorsRankInCompetition x, CompetitorsRankInCompetition y, int index)
        {
            if (index < x.competitor.RaceResults.Count && index < y.competitor.RaceResults.Count)
            {
                CompetitorResult a, b;
                a = x.competitor.RaceResults[index];
                b = y.competitor.RaceResults[index];

                if (a.Discarded || b.Discarded)
                {
                    return CompareRaceResult(x, y, index + 1);  //both of race result discarded, compare as same
                }
                // while are raceresults same, compare next in raceresult list of competitor
                if (a.RaceRank == b.RaceRank)
                {
                    return CompareRaceResult(x, y, index + 1);
                }
                else
                {
                    return a.RaceRank.CompareTo(b.RaceRank);
                }
            }
            else
            {
                //terminus
                return 0;
            }
        }

    }
}
