using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailing
{
    /* Simple structure CompetitorResult holds one Competitor and his position in race*/
    class CompetitorResult
    {
        private Competitor comp;
        private int positionFinished;
        private float points;

        public int PositionFinished { get { return positionFinished; } }
        public float Points
        {
            get { return positionFinished; }
            set { points = value; }
        }

        internal Competitor Comp { get { return comp; } }

        public CompetitorResult(Competitor comp, int position)
        {
            this.comp = comp;
            this.positionFinished = position;
            this.points = 0F;
        }
    }
}
