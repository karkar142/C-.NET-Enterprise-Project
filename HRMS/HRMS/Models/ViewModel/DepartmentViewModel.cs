namespace HRMS.Models.ViewModel
{
    public class DepartmentViewModel
    {
        public string  Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ExtensionPhone { get; set; }
        public int TotalEmployees { get; set; }
        public string PositionId { get; set; }
        public string PositionInfo { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentInfo { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
