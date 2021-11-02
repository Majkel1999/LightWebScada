using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontEnd.Migrations
{
    public partial class OrganizationOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                schema: "common",
                table: "organization",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                schema: "common",
                table: "organization");
        }
    }
}
