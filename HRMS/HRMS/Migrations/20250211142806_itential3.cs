using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class itential3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttendancePolicies",
                table: "AttendancePolicies");

            migrationBuilder.RenameTable(
                name: "AttendancePolicies",
                newName: "AttendancePolicy");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeductionInAmount",
                table: "AttendancePolicy",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttendancePolicy",
                table: "AttendancePolicy",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttendancePolicy",
                table: "AttendancePolicy");

            migrationBuilder.RenameTable(
                name: "AttendancePolicy",
                newName: "AttendancePolicies");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeductionInAmount",
                table: "AttendancePolicies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttendancePolicies",
                table: "AttendancePolicies",
                column: "Id");
        }
    }
}
