using System;
using System.Collections.Generic;
using NhibernateRepository;

namespace RepositoryInterfaces
{
    public interface IRepo
    {
        IUnitOfWork CreateUnitOfWork();
        T Get<T>(object id);
        IList<T> All<T>();
        IList<T> Get<T>(Func<T, bool> exp);
        T First<T>(Func<T, bool> exp);
    }
}
