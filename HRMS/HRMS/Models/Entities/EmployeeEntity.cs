using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    [Table("Employee")]
    public class EmployeeEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOE { get; set; }
        public DateTime? DOR { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public string PhoneNo { get; set; }
        public string PositionId { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual PositionEntity Position { get; set; }
        public string DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual DepartmentEntity Department { get; set; }
        public string? UserId { get; set; }
    }
}
