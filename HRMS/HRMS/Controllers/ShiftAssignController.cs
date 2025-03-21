using HRMS.DAO;
using HRMS.Migrations;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class ShiftAssignController : Controller
    {
        public readonly HRMSApplicationDbContext _dbContext;
        public ShiftAssignController(HRMSApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            bindEmployeeData();
            bindShiftData();
            return View();
            
        }

        private void bindShiftData()
        {
            IList<ShiftViewModel>shifts=_dbContext.Shifts.Where(w=>!w.IsInActive).Select(s=>new ShiftViewModel
            {
                Id = s.Id,
                Name = s.Name,  
            }).ToList();
            ViewBag.Shifts =shifts;
        }

        private void bindEmployeeData()
        {
            IList<EmployeeViewModel>employeeViews = _dbContext.Employees.Where(w=>!w.IsInActive).Select(s=> new EmployeeViewModel
            {
                Id = s.Id,
                Name = s.Name+"/"+s.Email+"/"+s.Gender,
            }).ToList();
            ViewBag.EmployeeViews = employeeViews;
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Entry(ShiftAssignViewModel ui)
        {
            try
            {
                ShiftAssignEntity assignEntity = new ShiftAssignEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = ui.EmployeeId,
                    ShiftId = ui.ShiftId,
                    FromDate = ui.FromDate,
                    ToDate = ui.ToDate,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.ShiftAssigns.Add(assignEntity);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully save the record to the system";
                ViewData["Status"]=true;
                bindEmployeeData();
                bindShiftData();
            }
            catch (Exception e)
            {

                ViewData["Info"] = "Error occur when save the record to the system"+e.Message ;
                ViewData["Status"] = false;
            }
            return View();
        }
        public IActionResult List()
        {
            IList<ShiftAssignViewModel>shiftAssignViews=(from e in _dbContext.ShiftAssigns
                                                         join f in _dbContext.Employees
                                                         on e.EmployeeId equals f.Id
                                                         join g in _dbContext.Shifts
                                                         on e.ShiftId equals g.Id
                                                         where !e.IsInActive && !f.IsInActive && !g.IsInActive
                                                         select new ShiftAssignViewModel
                                                         {
                                                             Id=e.Id,
                                                             EmployeeId=e.EmployeeId,
                                                             ShiftId=e.ShiftId,
                                                             FromDate=e.FromDate,
                                                             ToDate=e.ToDate,
                                                             EmployeeInfo=f.Name,
                                                             ShiftInfo=g.Name,
                                                             CreatedOn=e.CreatedAt,
                                                             UpdatedOn=e.ModifiedAt,
                                                         }).ToList();
            return View(shiftAssignViews);
        }
        [Authorize(Roles = "HR")]
        public IActionResult Edit(string Id)
        {
            ShiftAssignViewModel shiftAssign=_dbContext.ShiftAssigns.Where(w=>w.Id==Id && !w.IsInActive).Select(s=> new ShiftAssignViewModel
            {
                Id = s.Id,
                EmployeeId=s.EmployeeId,
                ShiftId=s.ShiftId,
                FromDate=s.FromDate,
                ToDate=s.ToDate,
                CreatedOn=s.CreatedAt,
                UpdatedOn=s.ModifiedAt,
            }).FirstOrDefault();
            bindShiftData();
            bindEmployeeData();
            return View(shiftAssign);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(ShiftAssignViewModel ui)
        {

            try
            {
                ShiftAssignEntity shiftAssignEntity = new ShiftAssignEntity()
                {
                    Id = ui.Id,
                    EmployeeId = ui.EmployeeId,
                    ShiftId = ui.ShiftId,
                    FromDate = ui.FromDate,
                    ToDate = ui.ToDate,
                    CreatedAt=ui.CreatedOn,
                    ModifiedAt=DateTime.Now,

                };
                _dbContext.ShiftAssigns.Update(shiftAssignEntity);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully update the record to the system";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {

                ViewData["Info"] = "Error occur update the record"+e.Message;
                ViewData["Stuatus"] = false;
            }
       
            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string Id)
        {
            ShiftAssignEntity shiftAssign = _dbContext.ShiftAssigns.Find(Id);
            if (shiftAssign != null)
            {
                shiftAssign.IsInActive=true;
                _dbContext.ShiftAssigns.Update(shiftAssign);
                _dbContext.SaveChanges();
                TempData["Info"] = "Successfully delete the record";
            }
            else
            {
                TempData["Info"] = "Error Occurr when delete the record";
            }
            return RedirectToAction("List");
        }
    }
}
