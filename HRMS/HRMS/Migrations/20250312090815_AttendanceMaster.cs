using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShiftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLate = table.Column<bool>(type: "bit", nullable: false),
                    IsEarlyOut = table.Column<bool>(type: "bit", nullable: false),
                    IsLeave = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceMaster_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceMaster_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceMaster_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceMaster_DepartmentId",
                table: "AttendanceMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceMaster_EmployeeId",
                table: "AttendanceMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceMaster_ShiftId",
                table: "AttendanceMaster",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceMaster");
        }
    }
}
