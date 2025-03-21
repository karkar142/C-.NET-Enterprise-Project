namespace HRMS.Models.ViewModel
{
    public class ShiftAssignViewModel
    {
        public string Id { get; set; } //for edit update and delete
        public string EmployeeId { get; set; }
        public string EmployeeInfo { get; set; }
        public string ShiftId { get; set; }
        public string ShiftInfo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
