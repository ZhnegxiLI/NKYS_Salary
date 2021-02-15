using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Validity = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cycle",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Label = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Validity = table.Column<bool>(nullable: false),
                    StandardWorkingHours = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    LastCalculedSalaryTime = table.Column<DateTime>(nullable: true),
                    ValidationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExportConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    ExportName = table.Column<string>(nullable: true),
                    ExportModel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalariesCalculModel",
                columns: table => new
                {
                    SalaryId = table.Column<long>(nullable: false),
                    EmployeName = table.Column<string>(nullable: true),
                    CycleLabel = table.Column<string>(nullable: true),
                    GroupLabel = table.Column<string>(nullable: true),
                    WorkingHours = table.Column<decimal>(nullable: true),
                    WorkingHoursDay = table.Column<decimal>(nullable: true),
                    WorkingHoursNight = table.Column<decimal>(nullable: true),
                    WorkingHoursHoliday = table.Column<decimal>(nullable: true),
                    WorkingScore = table.Column<decimal>(nullable: true),
                    AbsentHours = table.Column<decimal>(nullable: true),
                    DeferredHolidayHours = table.Column<decimal>(nullable: true),
                    WorkingDays = table.Column<decimal>(nullable: true),
                    BasicSalary = table.Column<decimal>(nullable: true),
                    OvertimePay = table.Column<decimal>(nullable: true),
                    AbsentDeduct = table.Column<decimal>(nullable: true),
                    PositionPay = table.Column<decimal>(nullable: true),
                    TransportFee = table.Column<decimal>(nullable: true),
                    FullPresencePay = table.Column<decimal>(nullable: true),
                    SeniorityPay = table.Column<decimal>(nullable: true),
                    DormFee = table.Column<decimal>(nullable: true),
                    DormOtherFee = table.Column<decimal>(nullable: true),
                    OtherRewardFee = table.Column<decimal>(nullable: true),
                    OtherPenaltyFee = table.Column<decimal>(nullable: true),
                    SocialSercurityFee = table.Column<decimal>(nullable: true),
                    SelfPaySocialSercurityFee = table.Column<decimal>(nullable: true),
                    HousingReservesFee = table.Column<decimal>(nullable: true),
                    NetSalary = table.Column<decimal>(nullable: true),
                    SalaryTax = table.Column<decimal>(nullable: true),
                    FinalSalary = table.Column<decimal>(nullable: true),
                    Salary_FixPart = table.Column<decimal>(nullable: true),
                    Salary_VariablePart = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SalaryCalculLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CalculTime = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    PeriodId = table.Column<long>(nullable: false),
                    StatusSuccess = table.Column<bool>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryCalculLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionValue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CycleId = table.Column<long>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    ProductionValueTypeId = table.Column<int>(nullable: false),
                    Validity = table.Column<bool>(nullable: false)
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: false),
                    SharePropotion = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GroupVariableSharePropotion = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ProductionValueTypeId = table.Column<int>(nullable: true),
                    IsFixSalary = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    GroupsId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntreEntrepriseDate = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    TechnicalLevel = table.Column<decimal>(nullable: true),
                    FixSalary = table.Column<decimal>(nullable: true),
                    IsTemporaryEmploye = table.Column<bool>(nullable: false),
                    DepartDate = table.Column<DateTime>(nullable: true),
                    DormFee = table.Column<decimal>(nullable: true),
                    HasTransportFee = table.Column<bool>(nullable: false),
                    IsChefOfGroup = table.Column<bool>(nullable: false),
                    PositionPay = table.Column<decimal>(nullable: true),
                    SocialSercurityFee = table.Column<decimal>(nullable: true),
                    SelfPaySocialSercurityFee = table.Column<decimal>(nullable: true),
                    HousingReservesFee = table.Column<decimal>(nullable: true),
                    SeniorityPay = table.Column<decimal>(nullable: true),
                    DeductionPercentage = table.Column<decimal>(nullable: true)
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    EmployeId = table.Column<long>(nullable: false),
                    DeductionSharePropotion = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LinkedProductionValueTypeId = table.Column<int>(nullable: true)
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CycleId = table.Column<long>(nullable: false),
                    EmployeId = table.Column<long>(nullable: false),
                    WorkingHours = table.Column<decimal>(nullable: true),
                    WorkingHoursDay = table.Column<decimal>(nullable: true),
                    WorkingHoursNight = table.Column<decimal>(nullable: true),
                    WorkingHoursHoliday = table.Column<decimal>(nullable: true),
                    WorkingScore = table.Column<decimal>(nullable: true),
                    AbsentHours = table.Column<decimal>(nullable: true),
                    DeferredHolidayHours = table.Column<decimal>(nullable: true),
                    WorkingDays = table.Column<decimal>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    BasicSalary = table.Column<decimal>(nullable: true),
                    OvertimePay = table.Column<decimal>(nullable: true),
                    AbsentDeduct = table.Column<decimal>(nullable: true),
                    PositionPay = table.Column<decimal>(nullable: true),
                    TransportFee = table.Column<decimal>(nullable: true),
                    FullPresencePay = table.Column<decimal>(nullable: true),
                    SeniorityPay = table.Column<decimal>(nullable: true),
                    DormFee = table.Column<decimal>(nullable: true),
                    DormOtherFee = table.Column<decimal>(nullable: true),
                    OtherRewardFee = table.Column<decimal>(nullable: true),
                    OtherPenaltyFee = table.Column<decimal>(nullable: true),
                    SocialSercurityFee = table.Column<decimal>(nullable: true),
                    SelfPaySocialSercurityFee = table.Column<decimal>(nullable: true),
                    HousingReservesFee = table.Column<decimal>(nullable: true),
                    NetSalary = table.Column<decimal>(nullable: true),
                    SalaryTax = table.Column<decimal>(nullable: true),
                    FinalSalary = table.Column<decimal>(nullable: true),
                    ValidatedBy = table.Column<long>(nullable: true),
                    ValidatedOn = table.Column<DateTime>(nullable: true),
                    Validity = table.Column<bool>(nullable: false),
                    FinalValidatedBy = table.Column<long>(nullable: true),
                    FinalValidatedOn = table.Column<DateTime>(nullable: true),
                    FinalValidity = table.Column<bool>(nullable: false)
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmployeDeductionConfiguration");

            migrationBuilder.DropTable(
                name: "ExportConfiguration");

            migrationBuilder.DropTable(
                name: "ProductionValue");

            migrationBuilder.DropTable(
                name: "SalariesCalculModel");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "SalaryCalculLog");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

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
