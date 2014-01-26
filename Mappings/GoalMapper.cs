using System.Linq;
using Behaviours.Enums;
using Behaviours.GoalBehaviours;
using Behaviours.GoalStrategies;
using Models;
using Repository.Models;

namespace Mappings
{
    public class GoalMapper
    {
        public static Goal Map(GoalEntity entity)
        {
            if (entity == null) return null;

            return new Goal
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    StartDate = entity.StartDate,
                    ShortName = entity.ShortName,
                    HexColour =  entity.HexColour,
                    Category = CategoryMapper.Map(entity.Category),
                    ChangeValue = entity.ChangeValue,
                    IntervalDuration = (GoalDurationType) entity.IntervalDurationId,
                    BehaviourType = (GoalBehaviourType) entity.EnumGoalBehaviourId,
                    Behaviour = GoalBehaviourFactory.Create((GoalBehaviourType) entity.EnumGoalBehaviourId),
                    UnitDescription = entity.UnitDescription,
                    GoalType = (GoalType) entity.EnumGoalTypeId,
                    Strategy = GoalTypeStrategyFactory.Create((GoalType) entity.EnumGoalTypeId),
                    Intervals = entity.Intervals.Select(GoalIterationMapper.Map).OrderBy(i => i.StartDate).ToList()                    
                };
        }

        public static GoalEntity Map(Goal entity)
        {
            if (entity == null) return null;

            return new GoalEntity
            {
                Id = entity.Id,
                Name = string.IsNullOrWhiteSpace(entity.Name) ? string.Empty : entity.Name,
                
                StartDate = entity.StartDate,
                ShortName = entity.ShortName,
                HexColour = entity.HexColour,
                Category = CategoryMapper.Map(entity.Category),
                ChangeValue = entity.ChangeValue,
                IntervalDurationId = (int) entity.IntervalDuration,
                EnumGoalBehaviourId = (int) entity.BehaviourType,
                EnumGoalTypeId = (int) entity.GoalType,
                UnitDescription = string.IsNullOrWhiteSpace(entity.UnitDescription) ? string.Empty : entity.UnitDescription,

                Intervals = entity.Intervals.Select(GoalIterationMapper.Map).OrderBy(i => i.StartDate).ToList()                
            };
        }
    }
}
