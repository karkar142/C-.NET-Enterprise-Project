using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class PayRollController : Controller
    {
        private readonly HRMSApplicationDbContext _dbContext;

        public PayRollController(HRMSApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public ActionResult List()
        {
            IList<PayRollViewModel>payRolls=(from d in _dbContext.PayRolls
                                             join e in _dbContext.Employees
                                             on d.EmployeeId equals e.Id
                                             join f in _dbContext.Departments
                                             on d.DepartmentId equals f.Id
                                             select new PayRollViewModel
                                             {
                                                 Id=d.Id,
                                                 FromDate=d.FromDate,
                                                 ToDate=d.ToDate,
                                                 EmployeeInfo=e.Name+"/"+e.Code,
                                                 BasicSalary=e.BasicSalary,
                                                 DepartmentInfo=f.Code+"/"+f.Description,
                                                 GrossPay=d.GrossPay,
                                                 NetPay=d.NetPay,
                                                 AttendanceDays=d.AttendanceDays,
                                                 AttendanceDeduction=d.AttendanceDeduction,
                                                 Allowance=d.Allowance,
                                                 Deduction=d.Deduction,
                                                 IncomeTax=d.IncomeTax,
                                             }).ToList();
                                             
            return View(payRolls);
        }

        public IActionResult PayRollProcess()
        {
            ViewBag.Employees=_dbContext.Employees.ToList();
            ViewBag.Departments=_dbContext.Departments.ToList();
            return View();
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult PayRollProcess(PayRollProcessViewModel Ui)
        {
            try
            {
                List<AttendanceMaterCalculateViewModel> calculateViewModels = new List<AttendanceMaterCalculateViewModel>();
                if (Ui.DepartmentId != null)
                {
                    List<AttendanceMasterEntity>attendances=_dbContext.attendanceMasters.Where(w=>w.DepartmentId == Ui.DepartmentId &&(w.AttendanceDate<=Ui.ToDate)).OrderBy(o=>o.AttendanceDate).ToList();
                    List<AttendanceMasterEntity>distamctEmployees=attendances.DistinctBy(o=>o.EmployeeId).ToList();
                    foreach(AttendanceMasterEntity d in distamctEmployees)
                    {
                        AttendanceMaterCalculateViewModel calculateDate=new AttendanceMaterCalculateViewModel();
                        calculateDate.DepartmentId=d.DepartmentId;
                        calculateDate.EmployeeId=d.EmployeeId;
                        calculateDate.FromDate=Ui.FromDate;
                        calculateDate.ToDate=Ui.ToDate;
                        calculateDate.BasicPay = _dbContext.Employees.Find(d.EmployeeId).BasicSalary;
                        calculateDate.LateCount=attendances.Where(w=>w.EmployeeId==d.EmployeeId && w.IsLate && (w.AttendanceDate>=Ui.FromDate && w.AttendanceDate<=Ui.ToDate)).Count();
                        calculateDate.EarlyOutCount= attendances.Where(w => w.EmployeeId == d.EmployeeId && w.IsEarlyOut && (w.AttendanceDate >= Ui.FromDate && w.AttendanceDate <= Ui.ToDate)).Count();
                        calculateDate.LeaveCount= attendances.Where(w => w.EmployeeId == d.EmployeeId && w.IsLeave && (w.AttendanceDate >= Ui.FromDate && w.AttendanceDate <= Ui.ToDate)).Count();
                        calculateDate.AttendanceDays=attendances.Where(w=>w.EmployeeId==d.EmployeeId && w.IsLeave==false && (w.AttendanceDate >= Ui.FromDate && w.AttendanceDate <= Ui.ToDate)).Count();
                        calculateViewModels.Add(calculateDate);
                    }
                    List<PayRollEntity> payRolls = CalculatePayRoll(calculateViewModels);
                    _dbContext.PayRolls.AddRange(payRolls);
                    _dbContext.SaveChanges();
                    TempData["Info"] = "Successfully save a record to the system";

                }
            }
            catch (Exception e)
            {
                TempData["Info"] = "Error occur when saving a record to the system";
            }
            return RedirectToAction("List");
        }
        private List<PayRollEntity>CalculatePayRoll(List<AttendanceMaterCalculateViewModel> attendanceMaterCalculateViewModel)
        {
            List<PayRollEntity> payRolls = new List<PayRollEntity>();
            decimal IncomeTax=2000, allowance=30000, deduction =10000;
            int workingDays = 30;
            foreach(AttendanceMaterCalculateViewModel cal in  attendanceMaterCalculateViewModel)
            {
                PayRollEntity payRoll = new PayRollEntity();
                payRoll.Id=Guid.NewGuid().ToString();
                payRoll.CreatedAt=DateTime.Now;
                payRoll.FromDate=cal.FromDate;
                payRoll.ToDate=cal.ToDate;
                payRoll.EmployeeId=cal.EmployeeId;
                payRoll.DepartmentId=cal.DepartmentId;  
                payRoll.IncomeTax=IncomeTax;
                payRoll.Allowance=allowance;
                payRoll.Deduction=deduction;
                payRoll.AttendanceDays=cal.AttendanceDays;
                decimal PayPerDay = (cal.BasicPay / workingDays);
               // payRoll.AttendanceDeduction=PayPerDay;
                payRoll.GrossPay=((PayPerDay)*cal.AttendanceDays)+allowance-payRoll.AttendanceDeduction-deduction;
                payRoll.NetPay=payRoll.GrossPay-payRoll.IncomeTax;
                payRolls.Add(payRoll);

            }
            return payRolls;
        }
        private decimal CalculateAttendanceDeductionByAttendancePolicy(string EmployeeId, decimal PayPerDay, int LateCount, int EarlyOutCount)
        {
            decimal attendanceDeduction = 0;
            var attendancePolicy = (from d in _dbContext.AttendancePolicies
                                    join e in _dbContext.Shifts
                                    on d.Id equals e.AttendancePolicyID
                                    join f in _dbContext.ShiftAssigns
                                    on e.Id equals f.ShiftId
                                    where f.EmployeeId == EmployeeId
                                    select d).FirstOrDefault();
            if (attendancePolicy != null)
            {
                if (LateCount > attendancePolicy.NumberOfLateTime || attendancePolicy?.NumberOfEarlyOutTime < EarlyOutCount)
                {
                    attendanceDeduction = attendancePolicy?.DeductionInAmount ?? 0;
                }
                if (attendancePolicy?.DeductionInDay > 0)
                {
                    attendanceDeduction += (PayPerDay * attendancePolicy?.DeductionInDay) ?? 0;
                }
            }return attendanceDeduction;
        }
    }
}
