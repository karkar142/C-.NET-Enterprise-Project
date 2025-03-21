using HRMS.DAO;
using HRMS.Repositories.Domain;

namespace HRMS.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSApplicationDbContext _dbContext;

        public UnitOfWork(HRMSApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IPositionRepository _positionRepository;
        public IPositionRepository PositionRepository
        {
            get
            {
                return _positionRepository= _positionRepository??new PositionRepository(_dbContext);
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Roolback()
        {
            _dbContext.Dispose();
        }
    }
}
