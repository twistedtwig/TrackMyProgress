using System.ComponentModel;

namespace Behaviours.Enums
{
    public enum GoalBehaviourType
    {
        [Description("Increment by Value")]
        IncrementValue = 1,

        [Description("Increment by Percentage")]
        IncrementPercentage = 2,

        [Description("Reduce by Value")]
        ReduceValue = 3,

        [Description("Reduce by Percentage")]
        ReducePercentage = 4,

        [Description("Don't Change")]
        None = 99
    }
}