using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    [Table("Shift")]
    public class ShiftEntity : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public TimeSpan LateAfter { get; set; }
        public TimeSpan EarlyOutBefore { get; set; }
        public string AttendancePolicyID { get; set; }
        [ForeignKey(nameof(AttendancePolicyID))]
        public virtual AttendancePolicyEntity AttendancePolicy { get; set; }    
    }
}
