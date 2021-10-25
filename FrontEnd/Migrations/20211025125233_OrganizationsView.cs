using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FrontEnd.Migrations
{
    public partial class OrganizationsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "viewobject",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    ViewJson = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viewobject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_viewobject_organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "common",
                        principalTable: "organization",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientconfigentity_OrganizationId",
                schema: "common",
                table: "clientconfigentity",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_viewobject_OrganizationId",
                schema: "common",
                table: "viewobject",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_clientconfigentity_organization_OrganizationId",
                schema: "common",
                table: "clientconfigentity",
                column: "OrganizationId",
                principalSchema: "common",
                principalTable: "organization",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientconfigentity_organization_OrganizationId",
                schema: "common",
                table: "clientconfigentity");

            migrationBuilder.DropTable(
                name: "viewobject",
                schema: "common");

            migrationBuilder.DropIndex(
                name: "IX_clientconfigentity_OrganizationId",
                schema: "common",
                table: "clientconfigentity");
        }
    }
}
