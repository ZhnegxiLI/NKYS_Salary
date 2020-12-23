using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class AddFinalSalaryIntoSalaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FinalSalary",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalSalary",
                table: "Salary");
        }
    }
}
