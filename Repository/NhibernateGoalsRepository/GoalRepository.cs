using System;
using System.Linq.Expressions;
using GoalInterfaces;
using NHibernate.Linq;
using NhibernateRepository;
using Repository.Models;
using System.Collections.Generic;
using System.Linq;

namespace NhibernateGoalsRepository
{
    public class GoalRepository : DbRepository, IGoalRepository
    {
        public GoalRepository(IDefinitionLoader definitionLoader, ISchemaConfigurationLoader schemaConfigurationLoader)
            : base(definitionLoader, schemaConfigurationLoader)
        {
        }

        public T Get<T>(int id)
        {
            return ReadOnlySession.Get<T>(id);
        }

        public IList<T> All<T>()
        {
            return ReadOnlySession.Query<T>().ToList();
        }

        public IList<TU> Select<T, TU>(Expression<Func<T, TU>> exp)
        {
            return ReadOnlySession.Query<T>().Select(exp).ToList();
        }

        public List<string> DistinctGoalNames()
        {
            return ReadOnlySession.Query<GoalEntity>().Select(g => g.Name).Distinct().ToList();
        }

        public List<string> DistinctGoalShortNames()
        {
            return ReadOnlySession.Query<GoalEntity>().Select(g => g.ShortName).Distinct().ToList();
        }

        public List<GoalEntity> GetGoals()
        {
            return ReadOnlySession.Query<GoalEntity>().ToList();
        }

        public List<CategoryEntity> GetCategories()
        {
            return ReadOnlySession.Query<CategoryEntity>().ToList();
        }

        public IList<T> Get<T>(Func<T, bool> exp)
        {
            return ReadOnlySession.Query<T>().Where(exp).ToList();
        }

        public T First<T>(Func<T, bool> exp)
        {
            return ReadOnlySession.Query<T>().Where(exp).FirstOrDefault();
        }
    }
}
