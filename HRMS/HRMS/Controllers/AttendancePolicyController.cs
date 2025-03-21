using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class AttendancePolicyController : Controller
    {
        public readonly HRMSApplicationDbContext _dbContext;
        public AttendancePolicyController(HRMSApplicationDbContext dbContext)
        {
             _dbContext= dbContext;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            return View();
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Entry(AttendancePolicyViewModel policyViewModel)
        {
            try
            {
                AttendancePolicyEntity entity = new AttendancePolicyEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = policyViewModel.Name,
                    NumberOfLateTime = policyViewModel.NumberOfLateTime,
                    NumberOfEarlyOutTime = policyViewModel.NumberOfEarlyOutTime,
                    DeductionInAmount = policyViewModel.DeductionInAmount,
                    DeductionInDay = policyViewModel.DeductionInDay,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.AttendancePolicies.Add(entity);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully save the record to the system.";
                ViewData["Status"]=true;
            }
            catch (Exception e)
            {
                ViewData["Info"]="Error occur when the record to the system."+e.Message;
                ViewData["Status"] = false;
            }
            
            return View();
        }
        public IActionResult List()
        {
            IList<AttendancePolicyViewModel>attendances=_dbContext.AttendancePolicies.Where(w=>!w.IsInActive).Select(s=> new AttendancePolicyViewModel
            {
                Id=s.Id,
                Name=s.Name,
                NumberOfLateTime=s.NumberOfLateTime,
                NumberOfEarlyOutTime=s.NumberOfEarlyOutTime,
                DeductionInAmount=s.DeductionInAmount,
                DeductionInDay=s.DeductionInDay,
                CreatedOn = s.CreatedAt,
                UpdatedOn=s.ModifiedAt,

            }).ToList();
            return View(attendances);
        }
        public IActionResult Edit(string id)
        {
            AttendancePolicyViewModel attendance=_dbContext.AttendancePolicies.Where(w=>w.Id==id && !w.IsInActive).Select(s=> new AttendancePolicyViewModel
            {
                Id = s.Id,
                Name=s.Name,
               NumberOfLateTime = s.NumberOfLateTime,
               NumberOfEarlyOutTime = s.NumberOfEarlyOutTime,
               DeductionInAmount = s.DeductionInAmount,
               DeductionInDay = s.DeductionInDay,
               CreatedOn = s.CreatedAt,
               UpdatedOn=s.ModifiedAt,
            }).FirstOrDefault();
            return View(attendance);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(AttendancePolicyViewModel attendance)
        {
            try
            {
                AttendancePolicyEntity attendancePolicy = new AttendancePolicyEntity()
                {
                    Id = attendance.Id,
                    Name = attendance.Name,
                    NumberOfLateTime = attendance.NumberOfLateTime,
                    NumberOfEarlyOutTime = attendance.NumberOfEarlyOutTime,
                    DeductionInAmount = attendance.DeductionInAmount,
                    DeductionInDay = attendance.DeductionInDay,
                    CreatedAt = attendance.CreatedOn,
                    ModifiedAt = DateTime.Now,

                };
                _dbContext.AttendancePolicies.Update(attendancePolicy);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully update the record to the system";
                ViewData["Status"] = true; 
            }
            catch (Exception e)
            {
                ViewData["Info"]= "Error occur when update the record to the system"+e.Message;
                ViewData["Status"]=false;
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string id)
        {
            try
            {
                AttendancePolicyEntity attendancePolicy = _dbContext.AttendancePolicies.Find(id);
                if (attendancePolicy != null)
                {
                    attendancePolicy.IsInActive = true;
                    _dbContext.AttendancePolicies.Update(attendancePolicy);
                    _dbContext.SaveChanges();
                    TempData["info"] = "Successfully Delete the record from the system";
                }
            }
            catch (Exception e)
            {

                TempData["info"] = "Error occour when delete record";
            }
            return RedirectToAction("List");
        }
    }
}
