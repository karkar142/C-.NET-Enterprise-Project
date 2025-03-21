using HRMS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.ViewModel
{
    public class EmployeeViewModel
    {
        public string Id { get; set; } //edit or delete 
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
        public string PositionInfo { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentInfo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UserId { get; set; }
    }
}
