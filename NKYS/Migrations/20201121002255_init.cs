using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cycle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Validity = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardWorkingHours = table.Column<int>(type: "int", nullable: true),
                    LastCalculedSalaryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CycleId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionValueTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionValue_Cycle_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    SharePropotion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductionValueTypeId = table.Column<long>(type: "bigint", nullable: true),
                    IsFixSalary = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employe",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntreEntrepriseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<long>(type: "bigint", nullable: true),
                    TechnicalLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SelfPaySocialSercurity = table.Column<bool>(type: "bit", nullable: true),
                    SelfPayHousingReserves = table.Column<bool>(type: "bit", nullable: true),
                    HasDorm = table.Column<bool>(type: "bit", nullable: true),
                    TransportFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PositionPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsChefOfGroup = table.Column<bool>(type: "bit", nullable: true),
                    SeniorityPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeductionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsTemporaryEmploye = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employe_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeDeductionConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeId = table.Column<long>(type: "bigint", nullable: false),
                    LinkedProductionValueTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeDeductionConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeDeductionConfiguration_Employe_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CycleId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeId = table.Column<long>(type: "bigint", nullable: false),
                    WorkingHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkingScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbsentHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SocialSercurityFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HousingReserves = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FullPresencePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OvertimePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbsentDeduct = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DormFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransportFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salary_Cycle_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salary_Employe_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employe_GroupsId",
                table: "Employe",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeDeductionConfiguration_EmployeId",
                table: "EmployeDeductionConfiguration",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DepartmentId",
                table: "Groups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionValue_CycleId",
                table: "ProductionValue",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_CycleId",
                table: "Salary",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeId",
                table: "Salary",
                column: "EmployeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeDeductionConfiguration");

            migrationBuilder.DropTable(
                name: "ProductionValue");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "Cycle");

            migrationBuilder.DropTable(
                name: "Employe");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
