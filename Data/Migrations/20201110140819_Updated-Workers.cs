using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Data.Migrations
{
    public partial class UpdatedWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Worker");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Worker",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Worker",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Job",
                table: "Worker",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Worker",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Worker",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Worker");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Worker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
