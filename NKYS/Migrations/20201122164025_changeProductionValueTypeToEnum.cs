using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class changeProductionValueTypeToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductionValueTypeId",
                table: "ProductionValue",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartDate",
                table: "Employe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "StandardWorkingHours",
                table: "Cycle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Cycle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartDate",
                table: "Employe");

            migrationBuilder.AlterColumn<long>(
                name: "ProductionValueTypeId",
                table: "ProductionValue",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StandardWorkingHours",
                table: "Cycle",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Cycle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
