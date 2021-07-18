using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontEnd.Migrations
{
    public partial class apikeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                schema: "common",
                table: "organization",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                schema: "common",
                table: "organization");
        }
    }
}
