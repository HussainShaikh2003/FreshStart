using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshStart.Migrations
{
    public partial class UpdateStudentProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GitHub",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GraduationYear",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "City",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "GitHub",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "GraduationYear",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "StudentProfiles");
        }
    }
}
