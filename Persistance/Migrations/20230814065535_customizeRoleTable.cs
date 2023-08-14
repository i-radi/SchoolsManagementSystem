using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class customizeRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Roles_RoleId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_RoleId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ActivityId",
                table: "Roles",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Activities_ActivityId",
                table: "Roles",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Activities_ActivityId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ActivityId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoleId",
                table: "Activities",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Roles_RoleId",
                table: "Activities",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
