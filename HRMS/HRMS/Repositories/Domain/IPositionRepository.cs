using HRMS.Models.Entities;
using HRMS.Repositories.Common;

namespace HRMS.Repositories.Domain
{
    public interface IPositionRepository:IBaseRepository<PositionEntity>
    {
        //Customization code here 
        bool IsAlreadyExist(string Code, string Name);
    }
}
