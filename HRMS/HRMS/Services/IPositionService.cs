using HRMS.Models.Entities;
using HRMS.Models.ViewModel;

namespace HRMS.Services
{
    public interface IPositionService
    {
        //Create
        void Create(PositionViewModel positionViewModel);
        //Reterive
        IEnumerable<PositionViewModel> GetAll();
        //GetById
        PositionViewModel GetById(string id);
        //Update
        void Update(PositionViewModel positionViewModel);
        //delete
        bool Delete(string id);
        //
        bool IsAlreadyExist(PositionViewModel positionViewModel);
    }
}
