using System.Collections.Generic;
using EntityModels;
using Goals.Models;
using System.Linq;
using Goals.Shared.Extensions;

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
                StartDate = entity.StartDate.ConvertToLocal(),
                EndDate = entity.EndDate.ConvertToLocal(),
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
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                Target = model.Target,
                Percentage = model.Percentage,

                Entries = model.Entries.Select(GoalRecordMapper.Map).ToList()
            };
        }



        public static void ConvertToLocalDate(GoalIterationEntity entity)
        {
            if (entity == null) return;

            entity.StartDate = entity.StartDate.ConvertToLocal();
            entity.EndDate= entity.EndDate.ConvertToLocal();

            GoalRecordMapper.ConvertToLocalDate(entity.Entries);
        }

        public static void ConvertToLocalDate(IList<GoalIterationEntity> entities)
        {
            if (entities == null) return;

            foreach (GoalIterationEntity entity in entities)
            {
                ConvertToLocalDate(entity);
            }
        }
    }
}
