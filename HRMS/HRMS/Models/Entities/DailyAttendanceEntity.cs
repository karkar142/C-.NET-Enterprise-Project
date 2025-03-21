using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    [Table("DailyAttendance")]
    public class DailyAttendanceEntity:BaseEntity
    {
        public DateTime AttendanceDate { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public string EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; }
        public string DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual DepartmentEntity Department { get; set; }
    }
}
