using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Repository.Models;

namespace NhibernateGoalsRepository.OverrideMappings
{
    public class GoalOverrideMapping : IAutoMappingOverride<GoalEntity>
    {
        public void Override(AutoMapping<GoalEntity> mapping)
        {
            mapping.References(m => m.Category).Cascade.SaveUpdate();
            mapping.HasMany(m => m.Intervals).Cascade.All();
        }        
    }
}
