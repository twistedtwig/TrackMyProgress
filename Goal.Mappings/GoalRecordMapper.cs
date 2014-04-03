using System.Collections.Generic;
using EntityModels;
using Goals.Models;
using Goals.Shared.Extensions;

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
                Date = entity.Date.ConvertToLocal()
            };
        }

        public static GoalRecordEntity Map(GoalRecord model)
        {
            if (model == null) return null;

            return new GoalRecordEntity
            {
                Id = model.Id,
                Amount = model.Amount,
                Date = model.Date.ToUniversalTime()
            };
        }


        public static void ConvertToLocalDate(GoalRecordEntity entity)
        {
            if(entity == null) return;

            entity.Date = entity.Date.ConvertToLocal();
        }

        public static void ConvertToLocalDate(IList<GoalRecordEntity> entities)
        {
            if (entities == null) return;

            foreach (var entity in entities)
            {
                ConvertToLocalDate(entity);
            }
        }
    }
}
