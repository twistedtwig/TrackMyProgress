using System;

namespace GoalWeb.Models
{
    public class GoalIterationModel
    {
        public int IterationId { get; set; }
        public int GoalId { get; set; }
        public string Units { get; set; }

        public double NewValue { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}