
using System.ComponentModel;

namespace Models
{
    public class GoalSummary
    {
        public int GoalId { get; set; }

        public string Title { get; set; }

        [DisplayName("No. Iterations")]
        public int NumberOfIterations { get; set; }

        [DisplayName("Avg Entries p/iteration")]
        public double AvgEntriesPerIteration { get; set; }

        [DisplayName("Avg Percentage to Target")]
        public double AvgPercentageToTarget { get; set; }

    }
}
