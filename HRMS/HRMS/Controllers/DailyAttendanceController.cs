using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class DailyAttendanceController : Controller
    {
        public readonly HRMSApplicationDbContext _dbContaxt;
        public DailyAttendanceController(HRMSApplicationDbContext dbContext)
        {
            _dbContaxt = dbContext;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            bindEmployeeId();
            bindDepartment();
            return View();
        }

        private void bindDepartment()
        {
            IList<DepartmentViewModel>departments=_dbContaxt.Departments.Where(w=>!w.IsInActive).Select(s=> new DepartmentViewModel
            {
                Id = s.Id,
                Code = s.Code+"/"+s.Description,
            }).ToList();
            ViewBag.Departments = departments;
        }

        private void bindEmployeeId()
        {
            IList<EmployeeViewModel>employees=_dbContaxt.Employees.Where(w=>!w.IsInActive).Select(s=> new EmployeeViewModel
            {
                Id = s.Id,
                Code = s.Code+"/"+s.Name,
            }).ToList();
            ViewBag.Employees = employees;
        }
        [Authorize(Roles = "HR")]
        [HttpPost]  
        public IActionResult Entry(DailyAttendanceViewModel Ui)
        {
            try
            {
                DailyAttendanceEntity daily = new DailyAttendanceEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    AttendanceDate = Ui.AttendanceDate,
                    InTime = Ui.InTime,
                    OutTime = Ui.OutTime,
                    EmployeeId = Ui.EmployeeId,
                    DepartmentId = Ui.DepartmentId,
                    CreatedAt = DateTime.Now,
                };
                _dbContaxt.DailyAttendances.Add(daily);
                _dbContaxt.SaveChanges();
                ViewData["Info"] = "Successfully save the record to the system";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {

                ViewData["Infp"] = "Error occur when save the record to the system" + e.Message;
                ViewData["Status"] = false;
            }
            bindDepartment();
            bindEmployeeId();
            return View();
        }
        public IActionResult List()
        {
            IList<DailyAttendanceViewModel>dailyAttendances=(from e in _dbContaxt.DailyAttendances
                                                             join f in _dbContaxt.Departments
                                                             on e.DepartmentId equals f.Id
                                                             join p in _dbContaxt.Employees
                                                             on e.EmployeeId equals p.Id
                                                             where !e.IsInActive && !f.IsInActive && !p.IsInActive
                                                             select new DailyAttendanceViewModel
            {
                Id=e.Id,
                AttendanceDate = e.AttendanceDate,
                InTime = e.InTime,
                OutTime = e.OutTime,
                EmployeeId = e.EmployeeId,
                EmployeeInfo=p.Code+"/"+p.Name,
                DepartmentId = e.DepartmentId,
                DepartmentInfo=f.Code+"/"+f.Description,
                CreatedOn=e.CreatedAt,
                UpdatedOn=e.ModifiedAt,
            }).ToList();
            return View(dailyAttendances);
        }
        
        public IActionResult Edit(string id)
        {
            DailyAttendanceViewModel daily=_dbContaxt.DailyAttendances.Where(w=>w.Id==id && !w.IsInActive).Select(s=>new DailyAttendanceViewModel
            {
                Id=s.Id,
                AttendanceDate=s.AttendanceDate,
                InTime=s.InTime,
                OutTime=s.OutTime,
                EmployeeId=s.EmployeeId,
                DepartmentId=s.DepartmentId,
                CreatedOn=s.CreatedAt,
                UpdatedOn=s.ModifiedAt,
            }).FirstOrDefault();
            bindEmployeeId();
            bindDepartment();
            return View(daily);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(DailyAttendanceViewModel ui)
        {
            try
            {
                DailyAttendanceEntity daily = new DailyAttendanceEntity()
                {
                    Id = ui.Id,
                    AttendanceDate = ui.AttendanceDate,
                    InTime = ui.InTime,
                    OutTime = ui.OutTime,
                    EmployeeId = ui.EmployeeId,
                    DepartmentId = ui.DepartmentId,
                    CreatedAt = ui.CreatedOn,
                    ModifiedAt = DateTime.Now,
                };
                _dbContaxt.DailyAttendances.Update(daily);
                _dbContaxt.SaveChanges();
                TempData["Info"] = "Update successfully";
                TempData["Status"] = true;
            }
            catch (Exception e)
            {
                TempData["Info"] = "Error occurred while updating the record: " + e.Message;
                TempData["Status"] = false;
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(String Id)
        {
            DailyAttendanceEntity daily = _dbContaxt.DailyAttendances.Find(Id);
            if(daily != null)
            {
                daily.IsInActive = true;
                _dbContaxt.DailyAttendances.Update(daily);
                _dbContaxt.SaveChanges();
                TempData["Info"] = "Successfully delete the record";
            }
            else
            {
                TempData["Info"] = "error occur when delete the record";
            }
            return RedirectToAction("List");
        }
    }
}
