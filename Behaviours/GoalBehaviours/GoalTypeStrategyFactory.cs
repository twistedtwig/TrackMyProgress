using Behaviours.Enums;
using System;

namespace Behaviours.GoalBehaviours
{
    public class GoalTypeStrategyFactory
    {
        public static IGoalTypeStrategy Create(GoalType goalType)
        {
            switch (goalType)
            {
                case GoalType.ChangeSomething:
                    return new ChangeSomethingGoalTypeStrategy();
                case GoalType.ReachSomething:
                    return new ReachSomethingGoalTypeStrategy();
                case GoalType.TrackSomething:
                    return new TrackSomethingGoalTypeStrategy();
                default:
                    throw new ArgumentOutOfRangeException("goalType");
            }
        }
    }
}
