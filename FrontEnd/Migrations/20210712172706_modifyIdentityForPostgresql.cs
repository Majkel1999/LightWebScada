using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FrontEnd.Migrations
{
    public partial class modifyIdentityForPostgresql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "private");

            migrationBuilder.CreateTable(
                name: "frontenduser",
                schema: "private",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frontenduser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identityrole",
                schema: "private",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityrole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identityuserclaim<string>",
                schema: "private",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityuserclaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identityuserclaim<string>_frontenduser_UserId",
                        column: x => x.UserId,
                        principalSchema: "private",
                        principalTable: "frontenduser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityuserlogin<string>",
                schema: "private",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityuserlogin<string>", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_identityuserlogin<string>_frontenduser_UserId",
                        column: x => x.UserId,
                        principalSchema: "private",
                        principalTable: "frontenduser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityusertoken<string>",
                schema: "private",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityusertoken<string>", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_identityusertoken<string>_frontenduser_UserId",
                        column: x => x.UserId,
                        principalSchema: "private",
                        principalTable: "frontenduser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityroleclaim<string>",
                schema: "private",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityroleclaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identityroleclaim<string>_identityrole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "private",
                        principalTable: "identityrole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityuserrole<string>",
                schema: "private",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityuserrole<string>", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_identityuserrole<string>_frontenduser_UserId",
                        column: x => x.UserId,
                        principalSchema: "private",
                        principalTable: "frontenduser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_identityuserrole<string>_identityrole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "private",
                        principalTable: "identityrole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "private",
                table: "frontenduser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "private",
                table: "frontenduser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "private",
                table: "identityrole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identityroleclaim<string>_RoleId",
                schema: "private",
                table: "identityroleclaim<string>",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_identityuserclaim<string>_UserId",
                schema: "private",
                table: "identityuserclaim<string>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_identityuserlogin<string>_UserId",
                schema: "private",
                table: "identityuserlogin<string>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_identityuserrole<string>_RoleId",
                schema: "private",
                table: "identityuserrole<string>",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identityroleclaim<string>",
                schema: "private");

            migrationBuilder.DropTable(
                name: "identityuserclaim<string>",
                schema: "private");

            migrationBuilder.DropTable(
                name: "identityuserlogin<string>",
                schema: "private");

            migrationBuilder.DropTable(
                name: "identityuserrole<string>",
                schema: "private");

            migrationBuilder.DropTable(
                name: "identityusertoken<string>",
                schema: "private");

            migrationBuilder.DropTable(
                name: "identityrole",
                schema: "private");

            migrationBuilder.DropTable(
                name: "frontenduser",
                schema: "private");
        }
    }
}
