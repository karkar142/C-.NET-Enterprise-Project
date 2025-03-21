using HRMS.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime ModifiedAt { get; set; }
        public string IpAddress { get; set; } = NetworkHelper.GetLocalIPAddress();
        public bool IsInActive { get; set; }
    }
}
