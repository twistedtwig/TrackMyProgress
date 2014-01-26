using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public class GoalIterationEntity
    {
        public GoalIterationEntity()
        {
            Entries = new List<GoalRecordEntity>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Target { get; set; }
        public double Achieved { get; set; }
        public double Percentage { get; set; }

        public IList<GoalRecordEntity> Entries { get; set; }
    }
}