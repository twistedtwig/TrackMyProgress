using System.ComponentModel;

namespace Behaviours.Enums
{
    public enum GoalType
    {
        [Description("Change the Value")]
        ChangeSomething = 1,

        [Description("Reach the Value")]
        ReachSomething = 2,

        [Description("Track a Value")]
        TrackSomething = 3
    }
}