using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.ViewModel
{
    public class AttendancePolicyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Number of Late Time cannot be negative.")]
        public int NumberOfLateTime { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Number of EarlyOut Time cannot be negative.")]
        public int NumberOfEarlyOutTime { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Deduction Amount cannot be negative.")]

        public decimal DeductionInAmount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Deduction In Day cannot be negative.")]
        public int DeductionInDay { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
