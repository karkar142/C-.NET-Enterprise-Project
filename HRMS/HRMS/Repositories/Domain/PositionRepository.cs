using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Repositories.Common;

namespace HRMS.Repositories.Domain
{
    public class PositionRepository : BaseRepository<PositionEntity>, IPositionRepository
    {
        private readonly HRMSApplicationDbContext _dbContext;

        public PositionRepository(HRMSApplicationDbContext dbContext) : base(dbContext)//Constructor changing
        {
            _dbContext = dbContext;
        }
        //Code your customize ur implatimation code here
        public bool IsAlreadyExist(string Code, string Name)
        {
           return  _dbContext.Positions.Where(w => (w.Code != Code && w.Name == Name) && !w.IsInActive).Any();
        }
    }
}
