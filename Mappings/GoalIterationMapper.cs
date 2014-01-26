using System.Linq;
using Models;
using Repository.Models;

namespace Mappings
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

        public static GoalIterationEntity Map(GoalIteration entity)
        {
            if (entity == null) return null;

            return new GoalIterationEntity
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
    }
}
