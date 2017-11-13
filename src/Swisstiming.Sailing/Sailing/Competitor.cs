﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Class represents one competitor in competition */
namespace Sailing
{
    public class Competitor : IComparable<Competitor>, IEnumerable<CompetitorResult>
    {

        private List<CompetitorResult> raceResults;

        public List<CompetitorResult> RaceResults { get => raceResults; }
        public string Name { get; set; }
        public float NetPoints { get; set; }
        public float TotalPoints { get; set; }

        public Competitor(string name)
        {
            this.raceResults = new List<CompetitorResult>();
            this.Name = name;
        }

        public override string ToString()
        {
            return "Competitor: " + Name;
        }

        /* Competitor can be compared on points, sorted competitors can be ranked in all competition - many races in competition*/
        public int CompareTo(Competitor other)
        {
            return this.NetPoints.CompareTo(other.NetPoints);
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
            hashCode = hashCode * -1521134295 + EqualityComparer<List<CompetitorResult>>.Default.GetHashCode(raceResults);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<CompetitorResult>>.Default.GetHashCode(RaceResults);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + NetPoints.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalPoints.GetHashCode();
            return hashCode;
        }

        public IEnumerator<CompetitorResult> GetEnumerator()
        {
            return this.raceResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
