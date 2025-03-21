using HRMS.Models.ReportModels;

namespace HRMS.Reportings
{
    public interface IReproting
    {
        IList<EmployeeDetail> EmployeeDetailReportBy(string fromEmployeeCode, string toEmployeeCode, string DepartmentId);
    }
}
