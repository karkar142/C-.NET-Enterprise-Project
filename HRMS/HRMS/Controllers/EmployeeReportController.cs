using HRMS.DAO;
using HRMS.Models.ReportModels;
using HRMS.Models.ViewModel;
using HRMS.Reportings;
using HRMS.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    
    public class EmployeeReportController : Controller
    {
        private readonly HRMSApplicationDbContext _dbContext;
        private readonly IReproting _reproting;

        public EmployeeReportController(HRMSApplicationDbContext dbContext,IReproting reproting)
        {
            this._dbContext = dbContext;
            this._reproting = reproting;
        }
        public IActionResult EmployeeDetailReport()
        {
            bindEmployeeData();
            bindDepartmentData();
            return View();
        }

        private void bindEmployeeData()
        {
            IList<EmployeeViewModel>employeeViews=_dbContext.Employees.Where(w=>!w.IsInActive).Select(s=> new EmployeeViewModel
            {
                Id = s.Id,
                Code = s.Code+"/"+s.Name, // Just the code for the value
                // Add a Name property to your ViewModel

            }).ToList();
            ViewBag.EmployeeViews = employeeViews;
        }

        private void bindDepartmentData()
        {
            IList<DepartmentViewModel> departmentViewModels = _dbContext.Departments.Where(w => !w.IsInActive).Select(s => new DepartmentViewModel
            {
                Id = s.Id,
                Code = s.Code + "/" + s.Description,
            }).ToList();
           ViewBag.DepartmentViews = departmentViewModels;  
        }
        [HttpPost]
        public IActionResult EmployeeDetailReport(string fromEmployeeCode, string  toEmployeeCode, string DepartmentCode)
        {
            IList<EmployeeDetail>employeeDetails=_reproting.EmployeeDetailReportBy(fromEmployeeCode, toEmployeeCode,DepartmentCode);
            string fileName = $"EmployeeDetailReport{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            if (employeeDetails.Any())
            {
                var FileContentslnBytes = ReportHelper.ExportToExcel(employeeDetails, fileName);
                var ContentType = "application/vnd.openxmlformat-officedecument.spreadsheet.sheet";
                ViewData["Info"] = "Successfully download the report";
                ViewData["Status"] = true;
                return File(FileContentslnBytes, ContentType,fileName);
                
            }
            else
            {
                bindDepartmentData();
                bindEmployeeData();
                ViewData["Info"] = "Error occur when download the report";
                ViewData["Status"] = false;
                return View();
            }
               
        }
    }
}
