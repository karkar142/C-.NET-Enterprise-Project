using HRMS.DAO;
using HRMS.Migrations;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class AttendanceMasterController : Controller
    {
        private readonly HRMSApplicationDbContext _dbContext;

        public AttendanceMasterController(HRMSApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult List()
        {
            var attendanceMasters=(from attemaster in _dbContext.attendanceMasters
                                   join Emp in _dbContext.Employees
                                   on attemaster.EmployeeId equals Emp.Id
                                   join Dep in _dbContext.Departments
                                   on attemaster.DepartmentId equals Dep.Id
                                   join sht in _dbContext.Shifts
                                   on attemaster.ShiftId equals sht.Id
                                   select new AttendanceMasterViewModel
                                   {
                                       Id = attemaster.Id,
                                       AttendanceDate=attemaster.AttendanceDate,
                                       InTime=attemaster.InTime,
                                       OutTime=attemaster.OutTime,
                                       EmployeeId=Emp.Code+"/"+Emp.Name,
                                       DepartmentId=Dep.Code+"/"+Dep.Description,
                                       ShiftId=sht.Name,
                                       IsLate=attemaster.IsLate,
                                       IsEarlyOut=attemaster.IsEarlyOut,
                                       IsLeave=attemaster.IsLeave,
                                   }).ToList();

            return View(attendanceMasters);
        }
        [Authorize(Roles = "HR")]
        public IActionResult DayEndProcess()
        {
            ViewBag.Shifts = (_dbContext.Shifts.ToList());
            ViewBag.Employees = (_dbContext.Employees.ToList());
            ViewBag.Departments = (_dbContext.Departments.ToList());
            return View();
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult DayEndProcess(AttendanceMasterViewModel Ui)
        {
            try
            {
                List<AttendanceMasterEntity> attendances = new List<AttendanceMasterEntity>();
                var DailyAttendanceWithShiftAssignDate = (from d in _dbContext.DailyAttendances
                                                          join e in _dbContext.ShiftAssigns
                                                          on d.EmployeeId equals e.EmployeeId
                                                          where e.EmployeeId == Ui.EmployeeId &&
                                                          (Ui.AttendanceDate >= e.FromDate && e.FromDate <= e.ToDate)
                                                          select new
                                                          {
                                                              dailyattendance = d,
                                                              Shiftassign = e,
                                                          }).ToList();
                foreach (var data in DailyAttendanceWithShiftAssignDate)
                {
                    ShiftEntity definedShift = _dbContext.Shifts.Where(s => s.Id == data.Shiftassign.Id).SingleOrDefault();
                    if (definedShift is not null)
                    {
                        AttendanceMasterEntity atm = new AttendanceMasterEntity();
                        atm.Id = Guid.NewGuid().ToString();
                        atm.CreatedAt = DateTime.Now;
                        atm.IsLeave = true;
                        atm.InTime = data.dailyattendance.InTime;
                        atm.OutTime = data.dailyattendance.OutTime;
                        atm.EmployeeId = data.dailyattendance.EmployeeId;
                        atm.ShiftId = definedShift.Id;
                        atm.DepartmentId = data.dailyattendance.DepartmentId;
                        atm.AttendanceDate = data.dailyattendance.AttendanceDate;

                        if (data.dailyattendance.InTime > definedShift.LateAfter)
                        {
                            atm.IsLate = true;
                        }
                        else
                        {
                            atm.IsLate = false;
                        }
                        //checking out the late status 
                        if (data.dailyattendance.OutTime < definedShift.EarlyOutBefore)
                        {
                            atm.IsEarlyOut = true;
                        }
                        else
                        {
                            atm.IsEarlyOut = false;
                        }
                        attendances.Add(atm);
                    }//end of the deifned shift not null 
                }
                _dbContext.attendanceMasters.AddRange(attendances);
                _dbContext.SaveChanges();
                ViewBag.Info = "successfully save a record to the system";
            }
            catch (Exception e)
            {

                ViewBag.Info = "Error occur when  saving a record  to the system";
            }

            return RedirectToAction("List");  
        }
        
    }
}
