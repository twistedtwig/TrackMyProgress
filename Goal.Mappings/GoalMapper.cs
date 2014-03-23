using System.Linq;
using Behaviours.GoalBehaviours;
using Behaviours.GoalStrategies;
using EntityModels;
using Goals.Shared.Enums;

namespace Goals.Mappings
{
    public class GoalMapper
    {
        public static Models.Goal Map(GoalEntity entity)
        {
            if (entity == null) return null;

            return new Models.Goal
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                StartDate = entity.StartDate,
                ShortName = entity.ShortName,
                HexColour = entity.HexColour,
                Category = CategoryMapper.Map(entity.Category),
                ChangeValue = entity.ChangeValue,
                IntervalDuration = (GoalDurationType)entity.IntervalDurationId,
                BehaviourType = (GoalBehaviourType)entity.EnumGoalBehaviourId,
                Behaviour = GoalBehaviourFactory.Create((GoalBehaviourType)entity.EnumGoalBehaviourId),
                UnitDescription = entity.UnitDescription,
                GoalType = (GoalType)entity.EnumGoalTypeId,
                Strategy = GoalTypeStrategyFactory.Create((GoalType)entity.EnumGoalTypeId),
                Intervals = entity.Intervals.Select(GoalIterationMapper.Map).OrderBy(i => i.StartDate).ToList()
            };
        }

        public static GoalEntity Map(Models.Goal model)
        {
            if (model == null) return null;

            return new GoalEntity
            {
                Id = model.Id,
                Name = string.IsNullOrWhiteSpace(model.Name) ? string.Empty : model.Name,
                UserId = model.UserId,

                StartDate = model.StartDate.ToUniversalTime(),
                ShortName = model.ShortName,
                HexColour = model.HexColour,
                Category = CategoryMapper.Map(model.Category),
                ChangeValue = model.ChangeValue,
                IntervalDurationId = (int)model.IntervalDuration,
                EnumGoalBehaviourId = (int)model.BehaviourType,
                EnumGoalTypeId = (int)model.GoalType,
                UnitDescription = string.IsNullOrWhiteSpace(model.UnitDescription) ? string.Empty : model.UnitDescription,

                Intervals = model.Intervals.Select(GoalIterationMapper.Map).OrderBy(i => i.StartDate).ToList()
            };
        }
    }
}

