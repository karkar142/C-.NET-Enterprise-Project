using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using HRMS.Repositories.Domain;
using HRMS.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Services
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Create(PositionViewModel positionViewModel)
        {
            PositionEntity positionEntity = new PositionEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Code = positionViewModel.Code,
                Name = positionViewModel.Name,
                Level = positionViewModel.level,
                CreatedAt = DateTime.Now,
            };
           _unitOfWork.PositionRepository.Create(positionEntity);
           _unitOfWork.Commit();
        }

        public bool Delete(string id)
        {
            try
            {
                PositionEntity positionEntity = _unitOfWork.PositionRepository.GetBy(w => w.Id == id).SingleOrDefault();
                if (positionEntity != null)
                {
                    positionEntity.IsInActive = true;
                    _unitOfWork.PositionRepository.Delete(positionEntity);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception e)
            {

                return false;
            }
            return true;    
        }

        public IEnumerable<PositionViewModel> GetAll()
        {
          return _unitOfWork.PositionRepository.GetAll().Where(w => !w.IsInActive).Select(s => new PositionViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                level = s.Level,
            }).AsEnumerable();
        }

        public PositionViewModel GetById(string id)
        {
            return _unitOfWork.PositionRepository.GetBy(w => w.Id == id && !w.IsInActive).Select(s => new PositionViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                level = s.Level,
                CreatedOn = s.CreatedAt,
                UpdatedOn = s.ModifiedAt,
            }).FirstOrDefault();
        }

        public bool IsAlreadyExist(PositionViewModel positionViewModel)
        {
            return _unitOfWork.PositionRepository.IsAlreadyExist(positionViewModel.Code, positionViewModel.Name);
        }

        public void Update(PositionViewModel positionViewModel)
        {
            PositionEntity positionEntity = new PositionEntity()
            {
                Id = positionViewModel.Id,
                Code = positionViewModel.Code,
                Name = positionViewModel.Name,
                Level = positionViewModel.level,
                CreatedAt = positionViewModel.CreatedOn,
                ModifiedAt = DateTime.Now,
            };
            _unitOfWork.PositionRepository.Update(positionEntity);
            _unitOfWork.Commit();
        }
    }
}
