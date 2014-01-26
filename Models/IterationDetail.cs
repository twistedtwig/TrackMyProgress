using System;

namespace Models
{
    public class IterationDetail
    {
        public int GoalId { get; set; }
        public int IterationId { get; set; }
        public int EntryId { get; set; }

        public string GoalName { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Units { get; set; }
    }
}
