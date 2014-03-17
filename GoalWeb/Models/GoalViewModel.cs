using System.Collections.Generic;
using System.Linq;
using Behaviours.GoalBehaviours;
using Behaviours.GoalStrategies;
using Goals.Models;
using Goals.Shared.Enums;

namespace GoalWeb.Models
{
    public class GoalViewModel : Goal
    {
        public GoalViewModel()
        {
            Categories = new List<Category>();
        }

        public GoalViewModel(Goal goal)
            : this()
        {
            Behaviour = goal.Behaviour;
            BehaviourType = goal.BehaviourType;
            Category = goal.Category;
            ChangeValue = goal.ChangeValue;
            GoalType = goal.GoalType;
            Id = goal.Id;
            IntervalDuration = goal.IntervalDuration;
            Intervals = goal.Intervals;
            Name = goal.Name;
            ShortName = goal.ShortName;
            HexColour = goal.HexColour;
            StartDate = goal.StartDate;
            UnitDescription = goal.UnitDescription;

            CategoryId = goal.Category.Id;
            GoalDurationTypeId = (int)goal.IntervalDuration;
            GoalTypeId = (int)goal.GoalType;
            GoalBehaviourTypeId = (int)goal.BehaviourType;
        }

        public GoalViewModel(Goal goal, IList<Category> categories) : this(goal)
        {
            Categories = categories;
        }

        public Goal RecreateGoal()
        {
            var g = this as Goal;
            g.Category = Categories.FirstOrDefault(c => c.Id == CategoryId);

            g.IntervalDuration = (GoalDurationType)GoalDurationTypeId;
            g.GoalType = (GoalType)GoalTypeId;
            g.BehaviourType = (GoalBehaviourType)GoalBehaviourTypeId;
            g.Behaviour = GoalBehaviourFactory.Create(g.BehaviourType);
            g.Strategy = GoalTypeStrategyFactory.Create(g.GoalType);
            g.Intervals = new List<GoalIteration>();

            return g;
        }

        public IList<Category> Categories { get; set; }

        public int CategoryId { get; set; }
        public int GoalDurationTypeId { get; set; }
        public int GoalTypeId { get; set; }
        public int GoalBehaviourTypeId { get; set; }
    }
}