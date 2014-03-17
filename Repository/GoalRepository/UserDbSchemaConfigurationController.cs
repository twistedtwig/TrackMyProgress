using EntityModels;
using GoalRepository.OverrideMappings;
using IdentityModelEntities;
using NhibernateRepository;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;

namespace GoalRepository
{
    public class UserDbSchemaConfigurationController : DbSchemaConfigurationController
    {
        public override void Configure(FluentConfiguration configuration)
        {
            configuration.Mappings(m => m.AutoMappings
                .Add((AutoMap.AssemblyOf<GoalEntity>())
                    .Conventions.Add(DefaultLazy.Never())
                    .Conventions.Add(DefaultCascade.All())
                    .UseOverridesFromAssemblyOf<GoalOverrideMapping>()
                )
                .Add((AutoMap.AssemblyOf<ApplicationUserEntity>())
                    .Conventions.Add(DefaultLazy.Never())
                    .Conventions.Add(DefaultCascade.All())
                    .UseOverridesFromAssemblyOf<GoalOverrideMapping>()
                )
            );
   
            var persistenceModel = new AutoPersistenceModel();
            persistenceModel.Conventions.Add(DefaultLazy.Never());
            persistenceModel.Conventions.Add(DefaultCascade.All());
            configuration.Mappings(m => m.UsePersistenceModel(persistenceModel));
        }
    }
}
