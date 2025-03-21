using HRMS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.ViewModel
{
    public class DailyAttendanceViewModel
    {
        public string Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeInfo {  get; set; }   
        public string DepartmentId { get; set; }
        public string DepartmentInfo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
