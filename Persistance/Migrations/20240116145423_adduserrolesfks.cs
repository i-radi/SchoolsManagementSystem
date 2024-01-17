using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class adduserrolesfks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryDate" },
                values: new object[] { new Guid("03b2775a-bcbb-46d3-909e-a0862f90e194"), new DateTime(2024, 2, 5, 14, 54, 19, 876, DateTimeKind.Utc).AddTicks(9119) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_OrganizationId",
                table: "UserRoles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_SchoolId",
                table: "UserRoles",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Organizations_OrganizationId",
                table: "UserRoles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Schools_SchoolId",
                table: "UserRoles",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Organizations_OrganizationId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Schools_SchoolId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_OrganizationId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_SchoolId",
                table: "UserRoles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryDate" },
                values: new object[] { new Guid("b5a9df44-4d5e-4824-8a5d-88689fe3782a"), new DateTime(2023, 9, 30, 10, 57, 40, 9, DateTimeKind.Utc).AddTicks(205) });
        }
    }
}
