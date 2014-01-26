using Behaviours.Enums;
using System;

namespace Behaviours.GoalStrategies
{
    public class GoalBehaviourFactory
    {

        public static IGoalBehaviourStrategy Create(GoalBehaviourType behaviourType)
        {
            switch (behaviourType)
            {
                case GoalBehaviourType.IncrementValue:
                    return new GoalBehaviourIncrementValueStrategy();
                case GoalBehaviourType.IncrementPercentage:
                    return new GoalBehaviourIncrementPercentStrategy();
                case GoalBehaviourType.ReduceValue:
                    return new GoalBehaviourReduceValueStrategy();
                case GoalBehaviourType.ReducePercentage:
                    return new GoalBehaviourReducePercentStrategy();
                case GoalBehaviourType.None:
                    return new GoalBehaviourNoneStrategy();
                default:
                    throw new ArgumentOutOfRangeException("behaviourType");
            }
        }
    }
}
