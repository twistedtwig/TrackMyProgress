namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourNoneStrategy : IGoalBehaviourStrategy
    {
        public double Execute(double sourceValue, double changeValue)
        {
            return sourceValue;
        }
    }
}
