using Models;
using Repository.Models;

namespace Mappings
{
    public class GoalRecordMapper
    {
        public static GoalRecord Map(GoalRecordEntity entity)
        {
            if (entity == null) return null;

            return new GoalRecord
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Date = entity.Date
            };
        }

        public static GoalRecordEntity Map(GoalRecord entity)
        {
            if (entity == null) return null;

            return new GoalRecordEntity
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Date = entity.Date
            };
        }
    }
}
