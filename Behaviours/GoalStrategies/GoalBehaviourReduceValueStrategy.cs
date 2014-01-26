using System;

namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourReduceValueStrategy : IGoalBehaviourStrategy
    {
        public double Execute(double sourceValue, double changeValue)
        {
            return sourceValue - changeValue;
        }
    }
}