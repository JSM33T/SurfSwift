using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfSwift.WorkerService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsValidUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tblActionProject",
                columns: table => new
                {
                    ActionProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblActionProject", x => x.ActionProjectId);
                    table.ForeignKey(
                        name: "FK_tblActionProject_tblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblActionTemplate",
                columns: table => new
                {
                    ActionTemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    ActionProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblActionTemplate", x => x.ActionTemplateId);
                    table.ForeignKey(
                        name: "FK_tblActionTemplate_tblActionProject_ActionProjectId",
                        column: x => x.ActionProjectId,
                        principalTable: "tblActionProject",
                        principalColumn: "ActionProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblActionTemplate_tblActionProject_UserId",
                        column: x => x.UserId,
                        principalTable: "tblActionProject",
                        principalColumn: "ActionProjectId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblActionProject_UserId",
                table: "tblActionProject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblActionTemplate_ActionProjectId",
                table: "tblActionTemplate",
                column: "ActionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tblActionTemplate_UserId",
                table: "tblActionTemplate",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblActionTemplate");

            migrationBuilder.DropTable(
                name: "tblActionProject");

            migrationBuilder.DropTable(
                name: "tblUser");
        }
    }
}
