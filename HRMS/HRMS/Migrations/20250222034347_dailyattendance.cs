using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    public partial class dailyattendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyAttendance",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false), // Change to string (for Guid as string)
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false), // Match Employee.Id length
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false), // Match Department.Id length
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyAttendance_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); // No cascading delete
                    table.ForeignKey(
                        name: "FK_DailyAttendance_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); // No cascading delete
                });

            // Optional: Create indexes on foreign key columns
            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendance_EmployeeId",
                table: "DailyAttendance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAttendance_DepartmentId",
                table: "DailyAttendance",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyAttendance");
        }
    }
}
