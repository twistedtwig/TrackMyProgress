using EntityModels;
using Goals.Models;
using System.Linq;

namespace Goals.Mappings
{
    public class GoalIterationMapper
    {
        public static GoalIteration Map(GoalIterationEntity entity)
        {
            if (entity == null) return null;

            return new GoalIteration
            {
                Id = entity.Id,
                Achieved = entity.Achieved,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Target = entity.Target,
                Percentage = entity.Percentage,

                Entries = entity.Entries.Select(GoalRecordMapper.Map).ToList()
            };
        }

        public static GoalIterationEntity Map(GoalIteration model)
        {
            if (model == null) return null;

            return new GoalIterationEntity
            {
                Id = model.Id,
                Achieved = model.Achieved,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Target = model.Target,
                Percentage = model.Percentage,

                Entries = model.Entries.Select(GoalRecordMapper.Map).ToList()
            };
        }
    }
}
