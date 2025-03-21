using HRMS.DAO;
using HRMS.Models.ReportModels;

namespace HRMS.Reportings
{
    public class Reporting : IReproting
    {
        private readonly HRMSApplicationDbContext _dbContext;

        public Reporting(HRMSApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<EmployeeDetail> EmployeeDetailReportBy(string fromEmployeeCode, string toEmployeeCode, string DepartmentId)
        {
            if(DepartmentId is not null)
            {
                var employeeDetail = (from e in _dbContext.Employees
                                      join d in _dbContext.Departments
                                      on e.DepartmentId equals d.Id
                                      join p in _dbContext.Positions
                                      on e.PositionId equals p.Id
                                      where !e.IsInActive && !d.IsInActive && ! p.IsInActive &&
                                      e.DepartmentId == DepartmentId
                                      select new EmployeeDetail
                                      {
                                          Code = e.Code,
                                          Name = e.Name,
                                          Email = e.Email,
                                          PhoneNo = e.PhoneNo,
                                          DOR = e.DOR.Value.ToString("yyyy-MM-dd"),
                                          DOE = e.DOE.ToString("yyyy-MM-dd"),
                                          DOB = e.DOB.ToString("yyyy-MM-dd"),
                                          Address = e.Address,
                                          BasicSalary = e.BasicSalary,
                                          DepartmentInfo = d.Code + "/" + d.Description,
                                          PositionInfo = p.Code + "/" + p.Name
                                      }).ToList();
                return employeeDetail;
            }
            else
            {
                var employeeDetail = (from e in _dbContext.Employees
                                      join d in _dbContext.Departments
                                      on e.DepartmentId equals d.Id
                                      join p in _dbContext.Positions
                                      on e.PositionId equals p.Id
                                      where !e.IsInActive && !d.IsInActive && !p.IsInActive &&
                                      (e.Code.CompareTo(fromEmployeeCode) >= 0 && e.Code.CompareTo(toEmployeeCode) <= 0)
                                      select new EmployeeDetail
                                      {
                                          Code = e.Code,
                                          Name = e.Name,
                                          Email = e.Email,
                                          PhoneNo = e.PhoneNo,
                                          DOR = e.DOR.Value.ToString("yyyy-MM-dd"),
                                          DOE = e.DOE.ToString("yyyy-MM-dd"),
                                          DOB = e.DOB.ToString("yyyy-MM-dd"),
                                          Address = e.Address,
                                          BasicSalary = e.BasicSalary,
                                          DepartmentInfo = d.Code + "/" + d.Description,
                                          PositionInfo = p.Code + "/" + p.Name
                                      }).ToList();
                return employeeDetail;
            }
        }
    }
}
