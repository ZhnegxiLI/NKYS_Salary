using Microsoft.EntityFrameworkCore.Migrations;

namespace NKYS.Migrations
{
    public partial class GroupVariableSharePropotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupVariableShare",
                table: "Groups",
                newName: "GroupVariableSharePropotion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupVariableSharePropotion",
                table: "Groups",
                newName: "GroupVariableShare");
        }
    }
}
