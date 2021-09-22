using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontEnd.Migrations.View
{
    public partial class ViewsNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "common",
                table: "viewobject",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "common",
                table: "viewobject");
        }
    }
}
