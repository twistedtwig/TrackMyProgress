using System;
using NhibernateGoalsRepository.OverrideMappings;
using NhibernateRepository;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;
using Repository.Models;

namespace NhibernateGoalsRepository
{
    public class UserDbSchemaConfigurationController : DbSchemaConfigurationController
    {
        public override void Configure(FluentConfiguration configuration)
        {
            configuration.Mappings(m => m.AutoMappings.Add((AutoMap.AssemblyOf<GoalEntity>())
                               .Conventions.Add(DefaultCascade.All())
                               .Conventions.Add(DefaultLazy.Never())
                               .UseOverridesFromAssemblyOf<GoalOverrideMapping>())
           );

            var persistenceModel = new AutoPersistenceModel();
            persistenceModel.Conventions.Add(DefaultLazy.Never());
            configuration.Mappings(m => m.UsePersistenceModel(persistenceModel));
        }
    }
}
