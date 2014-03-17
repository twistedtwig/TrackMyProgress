using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using IdentityModelEntities;

namespace GoalRepository.OverrideMappings
{
    public class IdentityUserOverrideMapping : IAutoMappingOverride<IdentityUserEntity>
    {
        public void Override(AutoMapping<IdentityUserEntity> mapping)
        {
            mapping.HasMany(m => m.Logins).KeyColumn("UserId").ForeignKeyConstraintName("FK_user_logins");
            mapping.HasMany(m => m.Roles).KeyColumn("UserId").ForeignKeyConstraintName("FK_user_Roles");
        }
    }
}
