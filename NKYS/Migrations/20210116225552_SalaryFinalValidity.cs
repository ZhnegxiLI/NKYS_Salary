using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class SalaryFinalValidity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FinalValidatedBy",
                table: "Salary",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalValidatedOn",
                table: "Salary",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinalValidity",
                table: "Salary",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalValidatedBy",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "FinalValidatedOn",
                table: "Salary");

            migrationBuilder.DropColumn(
                name: "FinalValidity",
                table: "Salary");
        }
    }
}
