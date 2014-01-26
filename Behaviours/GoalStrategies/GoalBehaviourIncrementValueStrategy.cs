namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourIncrementValueStrategy : IGoalBehaviourStrategy
    {
        public double Execute(double sourceValue, double changeValue)
        {
            return sourceValue + changeValue;
        }
    }
}