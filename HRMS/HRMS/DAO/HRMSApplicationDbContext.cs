using HRMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DAO
{
    public class HRMSApplicationDbContext:IdentityDbContext<IdentityUser,IdentityRole,string>
    {
        public HRMSApplicationDbContext(DbContextOptions<HRMSApplicationDbContext>options):base(options){}
        //Register Dbset entities
        public DbSet<PositionEntity> Positions { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<AttendancePolicyEntity> AttendancePolicies { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<DailyAttendanceEntity> DailyAttendances { get; set; }
        public DbSet<ShiftEntity> Shifts { get; set; }
        public DbSet<ShiftAssignEntity> ShiftAssigns { get; set; }
        public DbSet<AttendanceMasterEntity>attendanceMasters { get; set; }
        public DbSet<PayRollEntity> PayRolls { get; set; }


    }
}
