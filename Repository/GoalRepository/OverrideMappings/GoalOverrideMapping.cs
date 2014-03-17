using EntityModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace GoalRepository.OverrideMappings
{
    public class GoalOverrideMapping : IAutoMappingOverride<GoalEntity>
    {
        public void Override(AutoMapping<GoalEntity> mapping)
        {
//            mapping.References(m => m.Category).Cascade.SaveUpdate();
//            mapping.HasMany(m => m.Intervals).Cascade.All();
        }
    }
}
