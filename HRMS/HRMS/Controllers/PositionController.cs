using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            PositionViewModel viewModel = new PositionViewModel()
            {
                level = 1
            };
            return View(viewModel);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Entry(PositionViewModel positionViewModel)
        {
            try
            {
                var IsAlreadyExist = _positionService.IsAlreadyExist(positionViewModel);
                if (IsAlreadyExist)
                {
                    ViewData["Info"] = "This position is already exist in the system";
                    ViewData["Status"]=false;
                    return View(positionViewModel);
                }
                /*  _dbContext.Positions.Add(positionEntity);
                  _dbContext.SaveChanges();*/
                  _positionService.Create(positionViewModel);
                   ViewData["info"] = "Successfully save the record to the system";
                  ViewData["status"]=true;
            }
            catch (Exception e)
            {
                ViewData["info"]="Error occur when the record save to the system"+e.Message;
                ViewData["status"]=false;
                
            }
            return View(positionViewModel);
        }
        public IActionResult List()
        {
            var positons = _positionService.GetAll();
            return View(positons);
        }
        public IActionResult Edit(string id)
        {
           var positionViewModel=_positionService.GetById(id);
            return View(positionViewModel);

            
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(PositionViewModel position)
        {
            try
            {
                var IsAlreadyExist=_positionService.IsAlreadyExist(position);
                if(IsAlreadyExist)
                {
                    return View("Edit", position);
                }
                _positionService.Update(position);
                ViewData["info"] = "Successfully update the record to the system.";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {
                ViewData["info"] = "Error occur when the record update to the system." + e.Message;
                ViewData["Status"] = false;
                return View(position);
            }

            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string id)
        {
            try
            {
                    var status =_positionService.Delete(id);
                    TempData["info"] = "Delete success..!";
                TempData["Status"] = true;
                
            }
            catch (Exception e)
            {
                TempData["Status"]=false;
                TempData["info"] = "Error occur when delete the record" + e.Message;
            }
            return RedirectToAction("List");//when update success go to list view
        }
    }
}
