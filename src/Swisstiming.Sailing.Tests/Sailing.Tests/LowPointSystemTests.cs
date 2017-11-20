using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sailing.Tests
{
    public class LowPointSystemTests
    {

        [Fact]
        public void LowPointSeriesTest()
        {
            LowPointSystem lp = new LowPointSystem();
            Assert.Equal(lp.GetPointsFromPosition(1), 1);
            Assert.Equal(lp.GetPointsFromPosition(2), 2);

            Assert.Equal(lp.GetPointsFromPosition(5), 5);
            Assert.Equal(lp.GetPointsFromPosition(6), 6);

            Assert.Equal(lp.GetPointsFromPosition(100), 100);
            Assert.Equal(lp.GetPointsFromPosition(-1), 0);
        }
    }
}
