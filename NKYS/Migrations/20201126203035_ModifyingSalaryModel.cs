using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class ModifyingSalaryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WorkingHours",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeferredHolidayHours",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DormOtherFee",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SelfPaySocialSercurityFee",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SeniorityPay",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeferredHolidayHours",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "DormOtherFee",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "SelfPaySocialSercurityFee",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "SeniorityPay",
                table: "Salary");

            migrationBuilder.AlterColumn<decimal>(
                name: "WorkingHours",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
