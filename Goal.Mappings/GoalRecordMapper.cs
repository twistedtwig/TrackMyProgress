using EntityModels;
using Goals.Models;

namespace Goals.Mappings
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

        public static GoalRecordEntity Map(GoalRecord model)
        {
            if (model == null) return null;

            return new GoalRecordEntity
            {
                Id = model.Id,
                Amount = model.Amount,
                Date = model.Date
            };
        }
    }
}
