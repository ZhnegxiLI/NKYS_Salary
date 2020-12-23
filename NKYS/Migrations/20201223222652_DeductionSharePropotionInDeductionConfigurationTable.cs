using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class DeductionSharePropotionInDeductionConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeductionSharePropotion",
                table: "EmployeDeductionConfiguration",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductionSharePropotion",
                table: "EmployeDeductionConfiguration");
        }
    }
}
