
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Repository.Models;

namespace GoalInterfaces
{
    public interface IGoalRepository : IRepoBase
    {
        T Get<T>(int id);
        IList<T> All<T>();
        IList<T> Get<T>(Func<T, bool> exp);
        T First<T>(Func<T, bool> exp);

        IList<TU> Select<T, TU>(Expression<Func<T, TU>> exp);
        
            List<string> DistinctGoalNames();
        List<string> DistinctGoalShortNames();

        List<GoalEntity> GetGoals();
        List<CategoryEntity> GetCategories();
    }
}
