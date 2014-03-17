using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using NhibernateRepository;
using RepositoryInterfaces;

namespace GoalRepository
{
    public class DbRepo : DbRepository, IRepo
    {
        public DbRepo() : this(new UserDbSchemaDefinitionConfigLoader(), new UserDbSchemaConfigurationController())
        {

        }
        public DbRepo(IDefinitionLoader definitionLoader, ISchemaConfigurationLoader schemaConfigurationLoader) : base(definitionLoader, schemaConfigurationLoader)
        {
        }


        public T Get<T>(object id)
        {
            return ReadOnlySession.Get<T>(id);
        }

        public IList<T> All<T>()
        {
            return ReadOnlySession.Query<T>().ToList();
        }

        public IList<T> Get<T>(Func<T, bool> exp)
        {
            return ReadOnlySession.Query<T>().Where(exp).ToList();
        }

        public T First<T>(Func<T, bool> exp)
        {
            return ReadOnlySession.Query<T>().Where(exp).FirstOrDefault();
        }

        public IList<TU> Select<T, TU>(Expression<Func<T, TU>> exp)
        {
            throw new NotImplementedException();
        }
    }
}
