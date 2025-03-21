﻿namespace HRMS.Models.ReportModels
{
    public class EmployeeDetail
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DOE { get; set; }
        public string? DOR { get; set; }
        public string? Address { get; set; }
        public decimal BasicSalary { get; set; }
        public string PhoneNo { get; set; }
        public string PositionInfo { get; set; }
        public string DepartmentInfo { get; set; }
    }
}
