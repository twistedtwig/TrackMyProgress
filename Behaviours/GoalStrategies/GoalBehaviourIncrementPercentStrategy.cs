namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourIncrementPercentStrategy : IGoalBehaviourStrategy
    {
        public double Execute(double sourceValue, double changeValue)
        {
            double percent = (changeValue/100) + 1;
            return sourceValue*percent;
        }
    }
}