﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Class represents one competitor in competition */
namespace Sailing
{
    public class Competitor : IComparable<Competitor>
    {

        public List<CompetitorResult> RaceResults { get; private set; }
        public string Name { get; set; }
        public float NetPoints { get; set; }
        public float TotalPoints { get; set; }
        public int SumOfRanks{
            get
            {
                return SumRanks();
            }
            private set { }
        }

        public Competitor(string name)
        {
            RaceResults = new List<CompetitorResult>();
            this.Name = name;
        }

        /* Competitor can be compared on points, sorted competitors can be ranked in all competition - many races in competition*/
        public int CompareTo(Competitor other)
        {
            return this.NetPoints.CompareTo(other.NetPoints);
        }

        private int SumRanks()
        {
            int sum = 0;
            foreach(CompetitorResult cr in RaceResults)
            {
                //sum only not discarded raceRanks
                if (!cr.Discarded)
                {
                    sum += cr.RaceRank;
                }
            }
            SumOfRanks = sum;
            return sum;
        }

        /* Generated by visual studio */
        public override bool Equals(object obj)
        {
            var competitor = obj as Competitor;
            return competitor != null && this.GetHashCode() == obj.GetHashCode();
        }


        public override int GetHashCode()
        {
            var hashCode = -1926335857;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override string ToString()
        {
            return "Competitor: " + Name;
        }

    }
}
