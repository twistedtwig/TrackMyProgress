using NhibernateRepository;

namespace GoalInterfaces
{
    public interface IRepoBase
    {
        IUnitOfWork CreateUnitOfWork();
    }
}