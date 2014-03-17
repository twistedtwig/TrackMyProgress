using System;
using System.Collections.Generic;

namespace Goals.Models
{
    public class GoalIteration
    {
        public GoalIteration()
        {
            Percentage = Target > 0 ? (Achieved / Target * 100) : 0;
            Entries = new List<GoalRecord>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Target { get; set; }
        public double Achieved { get; set; }
        public double Percentage { get; set; }

        public IList<GoalRecord> Entries { get; set; }
    }
}