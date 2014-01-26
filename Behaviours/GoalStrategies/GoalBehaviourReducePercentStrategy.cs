
namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourReducePercentStrategy : IGoalBehaviourStrategy
    {
        public double Execute(double sourceValue, double changeValue)
        {
            double val = (changeValue / 100) * sourceValue;
            return sourceValue - val;
        }
    }
}