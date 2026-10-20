using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshStart.Migrations
{
    public partial class AddResumeTextToStudentProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeText",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeText",
                table: "StudentProfiles");
        }
    }
}
