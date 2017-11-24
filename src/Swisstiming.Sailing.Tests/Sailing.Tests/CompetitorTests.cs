using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class CompetitorTests
    {

        [Fact]
        public void Should_sum_ranks()
        {
            Competitor c = new Competitor("tested");
            for (int x = 1; x <= 4; x++)
            {
                CompetitorResult cr = new CompetitorResult(c, x);
                cr.RaceRank = x;
                c.RaceResults.Add(cr);
            }
            Assert.Equal(c.SumOfRanks, 10);
        }

        [Fact]
        public void Should_sum_ranks2()
        {
            Competitor c = new Competitor("tested");
            for (int x = 1; x <= 100; x++)
            {
                CompetitorResult cr = new CompetitorResult(c, x);
                cr.RaceRank = 2;
                c.RaceResults.Add(cr);
            }
            // 100 * 2 = 200
            Assert.Equal(c.SumOfRanks, 200);
        }

    }
}
