using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontEnd.Migrations
{
    public partial class Organizations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "common",
                table: "organizationmember",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "common",
                table: "organizationmember",
                newName: "UserId");
        }
    }
}
