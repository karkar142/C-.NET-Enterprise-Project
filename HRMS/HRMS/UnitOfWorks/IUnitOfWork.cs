using HRMS.Repositories.Domain;

namespace HRMS.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //register or declare here for your repositroies............................
        IPositionRepository PositionRepository { get; }
        //commit stage (insert update delete)
        void Commit();
        //Rollback transction
        void Roolback();
    }
}
