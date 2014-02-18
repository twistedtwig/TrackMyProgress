using System.Collections.Generic;

namespace Models
{
    public class IterationDetailInformationModel
    {
        public IterationDetailInformationModel()
        {
            Iterations = new List<IterationSummary>();
        }

        public int GoalId { get; set; }
        public IList<IterationSummary> Iterations { get; set; }

        public int InitialStart { get; set; }
        public int InitialEnd { get; set; }
    }

    public class IterationSummary
    {
        public int IterationId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
