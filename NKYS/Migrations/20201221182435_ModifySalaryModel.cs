using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class ModifySalaryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherFee",
                table: "Salary",
                newName: "SalaryTax");

            migrationBuilder.RenameColumn(
                name: "HousingReserves",
                table: "Salary",
                newName: "OtherRewardFee");

            migrationBuilder.AddColumn<decimal>(
                name: "BasicSalary",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HousingReservesFee",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetSalary",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherPenaltyFee",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValidatedBy",
                table: "Salary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValidatedOn",
                table: "Salary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Validity",
                table: "Salary",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "HousingReservesFee",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "OtherPenaltyFee",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "ValidatedBy",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "ValidatedOn",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "Salary");

            migrationBuilder.RenameColumn(
                name: "SalaryTax",
                table: "Salary",
                newName: "OtherFee");

            migrationBuilder.RenameColumn(
                name: "OtherRewardFee",
                table: "Salary",
                newName: "HousingReserves");
        }
    }
}
