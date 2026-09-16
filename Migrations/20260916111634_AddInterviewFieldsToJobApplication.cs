using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshStart.Migrations
{
    public partial class AddInterviewFieldsToJobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerNotes",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDate",
                table: "JobApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewLocation",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewTime",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerNotes",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "InterviewDate",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "InterviewLocation",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "InterviewTime",
                table: "JobApplications");
        }
    }
}
