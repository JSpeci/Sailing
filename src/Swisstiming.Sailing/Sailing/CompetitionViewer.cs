using System;
using System.Collections.Generic;
using System.Text;

namespace Sailing
{
    public static class CompetitionViewer
    {
        /* Console output string */
        public static string ViewTable(Competition competition)
        {
            StringBuilder sb = new StringBuilder();

            //Table header
            sb.Append(String.Format("{0,8}{1,8}", "Competitor", "Rank"));
            int raceNumber = 1;
            foreach (Race r in competition.Races)
            {
                sb.Append(String.Format("{0,8}", "Race" + raceNumber++.ToString()));
            }
            sb.Append(String.Format("{0,8}{1,8}{2,8}", "Sum" ,"Net", "Total"));
            sb.Append("\n");

            //Table content
            foreach (CompetitorsRankInCompetition cRank in competition.Ranks)
            {
                sb.Append(String.Format("{0,8}{1,8}", cRank.competitor.Name, cRank.rankInCompetition.ToString()));
                foreach (Race r in competition.Races)
                {
                    CompetitorResult cResTemp = null;
                    foreach (CompetitorResult cRes in r.RaceResult)
                    {
                        if (string.Equals(cRank.competitor.Name, cRes.Competitor.Name))
                        {
                            cResTemp = cRes;
                        }
                    }
                    sb.Append(String.Format("{0,8}", cResTemp.Discarded ? ("(" + cResTemp.RaceRank.ToString() + ")") : cResTemp.RaceRank.ToString()));
                }
                sb.Append(String.Format("{0,8}{1,8}{2,8}",cRank.competitor.SumOfRanks, cRank.competitor.NetPoints.ToString(), cRank.competitor.TotalPoints.ToString()));
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
