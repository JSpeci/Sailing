using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Sailing.Tests
{
    public class CompetitorsLoaderTests
    {

        /* If loader has 3 races loaded from direcotry*/
        [Fact]
        public void CountOfRacesInCompetitionTest()
        {
            Competition competition = CompetitorsLoader.LoadCompetitorsFromDirectoryPath("./data1/");
            Assert.Equal(3, competition.Races.Count);
        }      

        /* If any Competitors have zero races */
        [Fact]
        public void CompetitorsHaveRacesTest()
        {

            Competition competition = CompetitorsLoader.LoadCompetitorsFromDirectoryPath("./data1/");

            foreach (Competitor c in competition.Competitors)
            {
                Assert.NotNull(c.RaceResults);
                Assert.NotEmpty(c.RaceResults);
                Assert.Equal(competition.Races.Count, c.RaceResults.Count);
            }
        }

        /* If Competitors are filled with Races and positions - validate data drom csvs */
        [Fact]
        public void CompetitorsHavePositions()
        {
            Competition competition = CompetitorsLoader.LoadCompetitorsFromDirectoryPath("./data1/");

            foreach (Competitor c in competition.Competitors)
            {
                foreach(CompetitorResult cr in c.RaceResults)
                {
                    Assert.Equal(c, cr.Competitor);
                    Assert.True(cr.PositionFinished > 0);
                }
            }
        }

    }
}
