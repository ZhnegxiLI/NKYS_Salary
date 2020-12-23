using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class ModifySalaryAndEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelfPayHousingReserves",
                table: "Employe");

            migrationBuilder.RenameColumn(
                name: "TransportFee",
                table: "Employe",
                newName: "SocialSercurityFee");

            migrationBuilder.RenameColumn(
                name: "SelfPaySocialSercurity",
                table: "Employe",
                newName: "HasTransportFee");

            migrationBuilder.AddColumn<decimal>(
                name: "PositionPay",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingDays",
                table: "Salary",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HousingReservesFee",
                table: "Employe",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SelfPaySocialSercurityFee",
                table: "Employe",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionPay",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "HousingReservesFee",
                table: "Employe");

            migrationBuilder.DropColumn(
                name: "SelfPaySocialSercurityFee",
                table: "Employe");

            migrationBuilder.RenameColumn(
                name: "SocialSercurityFee",
                table: "Employe",
                newName: "TransportFee");

            migrationBuilder.RenameColumn(
                name: "HasTransportFee",
                table: "Employe",
                newName: "SelfPaySocialSercurity");

            migrationBuilder.AddColumn<bool>(
                name: "SelfPayHousingReserves",
                table: "Employe",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
