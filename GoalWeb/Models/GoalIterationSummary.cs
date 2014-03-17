using Goals.Models;

namespace GoalWeb.Models
{
    public class GoalIterationSummary
    {
        public GoalIterationSummary()
        {

        }

        public GoalIterationSummary(GoalIteration iteration, string description, string duration)
        {
            Achieved = iteration.Achieved;
            Target = iteration.Target;
            Percentage = iteration.Percentage;
            UnitDescription = description;
            DurationDesecription = duration;
        }

        public double Achieved { get; set; }
        public double Target { get; set; }
        public double Percentage { get; set; }
        public string UnitDescription { get; set; }
        public string DurationDesecription { get; set; }
    }
}