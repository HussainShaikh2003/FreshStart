
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshStart.Migrations
{
    public partial class AddSalaryRangeToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salary",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumSalary",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumSalary",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SalaryType",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Monthly");

            // Preserve existing salary values
            migrationBuilder.Sql(@"
                UPDATE Jobs
                SET MinimumSalary = TRY_CONVERT(decimal(18,2), Salary),
                    MaximumSalary = TRY_CONVERT(decimal(18,2), Salary),
                    SalaryType = 'Monthly'
                WHERE Salary IS NOT NULL
                  AND TRY_CONVERT(decimal(18,2), Salary) IS NOT NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumSalary",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "MinimumSalary",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SalaryType",
                table: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "Salary",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
